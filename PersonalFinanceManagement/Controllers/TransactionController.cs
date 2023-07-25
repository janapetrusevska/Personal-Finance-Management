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
using PersonalFinanceManagement.Models.Dto;
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
            if (invalidDatesMessage.Message.Count > 0)
            {
                return new ObjectResult(invalidDatesMessage);
            }

            var messages = new List<MessageDetails>();

            if (pageSize<0)
            {
                messages.Add(new MessageDetails
                {
                    StatusCode = 400,
                    Message = "PageSize variable has invalid value."
                });
            }
            if (page<0)
            {
                messages.Add(new MessageDetails
                {
                    StatusCode = 400,
                    Message = "Page variable has invalid value."
                });
            }
            if (messages.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = messages
                };
                return new ObjectResult(customMessage.Message);
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
                messages.Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 400,
                        Message = "No file uploaded"
                    }
                };
                return new ObjectResult(messages);
            }

            //reading all transactions from the file
            var transactions = _csvParserService.ReadingTransactionsFromFile(csvFile);

            var result = await _transactionService.ImportTransactions(transactions);

            if (result>0)
            {
                messages.Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 200,
                        Message = result+" new transactions have been added!"
                    }
                };
                return new ObjectResult(messages);
            }
            else
            {
                messages.Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 200,
                        Message = "All transactions have been updated!"
                    }
                };
                return new ObjectResult(messages);
            }
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransactions(string id, [FromBody] List<SingleCategorySplit> splits)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            var messages = new List<MessageDetails>();
            if (transaction.Id == null)
            {
                //if the transaction wasn't found
                messages.Add(new MessageDetails
                {
                    StatusCode = 400,
                    Message = "The transaction with that id doesn't exist!"
                });
                return new ObjectResult(messages);
            }
            
            var categoryCodes = splits.Select(s => s.CatCode).ToList();
            var existingCategories = new List<Category>();
            foreach (String s in categoryCodes)
            {
                var category = await _categoryService.GetCategoryByCode(s);
                if (category == null)
                {
                    messages.Add(new MessageDetails
                    {
                        StatusCode = 400,
                        Message = category.Code+" is an invalid value."
                    });
                }
                existingCategories.Add(category);
            }

            if (messages.Count > 0)
            {
                return new ObjectResult(messages);
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
                messages.Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 200,
                        Message = "The transaction with that id doesn't exist!"
                    }
                };
                return new ObjectResult(messages);
            }
            var catCode = transactionTask.Result.CatCode;
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
