using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Mappings;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Messages;
using PersonalFinanceManagement.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        ITransactionService _transactionService;
        ICategoryService _categoryService;

        private readonly IMapper _mapper;
        private readonly ILogger<TransactionController> _logger;
        private readonly PfmDbContext _context;
        private readonly ICsvParserService _csvParserService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, ICategoryService categoryService, PfmDbContext context, ICsvParserService csvParserService, IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _context = context;
            _csvParserService = csvParserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] string transactionKind, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.asc, [FromQuery] string? sortBy = null)
        {
            var invalidDatesMessage =  _transactionService.areTheDatesInvalid(startDate, endDate);
            if (invalidDatesMessage.Errors.Count > 0)
            {
                return new ObjectResult(invalidDatesMessage);
            }

            var errors = new List<ErrorDetails>();

            if (pageSize<0)
            {
                errors.Add(new ErrorDetails
                {
                    Error = "PageSize variable has invalid value."
                });
            }
            if (page<0)
            {
                errors.Add(new ErrorDetails
                {
                    Error = "Page variable has invalid value."
                });
            }
            if (errors.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error/s occured.",
                    Details = "You inserted an invalid value/s.",
                    Errors = errors
                };
                return new ObjectResult(customMessage);
            }

            var transactions = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);

            return Ok(transactions);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync(IFormFile csvFile)
        {
            var messages = new CustomMessage();
            if (csvFile == null || csvFile.Length == 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error occured",
                    Details = "No file has been uploaded",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "You haven't provided a csv file."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }

            //reading all transactions from the file
            var transactions = _csvParserService.ReadingTransactionsFromFile(csvFile);

            var result = await _transactionService.ImportTransactions(transactions);

            if (result>0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "Successful import!",
                    Details = result + " transactions have been added!",
                };
                return new ObjectResult(customMessage);
            }
            else
            {
                var customMessage = new CustomMessage
                {
                    Message = "Successful import!",
                    Details = "All transactions have been updated!",
                };
                return new ObjectResult(customMessage);
            }
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransactions(string id, [FromBody] List<SingleCategorySplit> splits)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            var errors = new List<ErrorDetails>();
            if (transaction.Id == null)
            {
                //if the transaction wasn't found
                var customMessage = new CustomMessage
                {
                    Message = "An error occured",
                    Details = "No transaction has been found",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "There is no transaction with the id you provided."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }
            
            var categoryCodes = splits.Select(s => s.CatCode).ToList();
            var existingCategories = new List<Category>();
            foreach (String s in categoryCodes)
            {
                var category = await _categoryService.GetCategoryByCode(s);
                if (category == null)
                {
                    errors.Add(new ErrorDetails
                    {
                        Error = s+" is an invalid value."
                    });
                }
                existingCategories.Add(category);
            }
            var amounts = splits.Select(s => s.Amount).ToList();
            var sum = 0.0;
            foreach(Double amount in amounts)
            {
                if (amount<0)
                {
                    errors.Add(new ErrorDetails
                    {
                        Error = amount + " is an invalid value."
                    });
                }
                sum += amount;
            }

            if (sum != transaction.Amount)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error/s occured.",
                    Details = "You inserted an invalid value/s.",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The amounts of the splits don't add up to the amount of the transaction"
                    }
                }
                };
                return new ObjectResult(customMessage);
            }

            if (errors.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error/s occured.",
                    Details = "You inserted an invalid value/s.",
                    Errors = errors
                };
                return new ObjectResult(customMessage);
            }

            Transaction transactionSplitted = await _transactionService.ImportSplitsInTransaction(transaction, splits);


            return Ok(transactionSplitted);
        }

        [HttpPost("{id}/categorize")]
        public IActionResult CategorizeTransactions(string id)
        {
            var messages = new CustomMessage();
            var transactionTask = _transactionService.GetTransactionById(id);
            if (transactionTask.Result.Id == null)
            {
                //if the transaction wasn't found
                var customMessage = new CustomMessage
                {
                    Message = "An error occured",
                    Details = "No transaction has been found",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "There is no transaction with the id you provided."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }
            var catCode = transactionTask.Result.CatCode;
            if(catCode == null)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error occured",
                    Details = "The transaction's category is null.",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The transaction with the id you provided has no category."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }
            var categoryTask = _categoryService.GetCategoryByCode(catCode);
            var category = categoryTask.Result;

            return Ok(category);
        }

        [HttpPost("auto-categorize")]
        public IActionResult AutoCategorizeTransactions()
        {
            return Ok();
        }
    }
}
