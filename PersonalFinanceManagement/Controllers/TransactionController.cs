using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Habanero.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
        public async Task<IActionResult> GetTransactions([FromQuery] string transactionKind, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.asc, [FromQuery] string? sortBy = null)
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
                    Message = "An error/s occurred.",
                    Details = "You inserted an invalid value/s.",
                    Errors = errors
                };
                return new BadRequestObjectResult(customMessage);
            }

            PagedSortedList<Transaction> transactions = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);

            if (transactions.TotalCount == 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error occurred",
                    Details = "No transaction has been found",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "There are no transactions in the database that satisfy the conditions."
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
            }
            return Ok(transactions);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "No file has been uploaded.",
                    Details = "You haven't provided a csv file."
                };
                return new BadRequestObjectResult(customMessage);
            }
            if (!csvFile.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase))
            {
                var customMessage = new CustomMessage
                {
                    Message = "No CSV file was uploaded.",
                    Details = "Only CSV files are allowed for import."
                };
                return new BadRequestObjectResult(customMessage);
            }

            //reading all transactions from the file
            var transactions = _csvParserService.ReadingTransactionsFromFile(csvFile);

            var result = await _transactionService.ImportTransactions(transactions);
            var Message = new CustomMessage
            {
                Message = "Successful import!",
                Details = "Number of transactions added: " + result[0] + "; Number of transactions updated: " + result[1]
            };
            return new ObjectResult(Message);
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
                    Message = "An error occurred",
                    Details = "No transaction has been found",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "There is no transaction with the id you provided."
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
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
                        Error = s+" is an invalid value for category code."
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
                        Error = amount + " is an invalid value for amount."
                    });
                }
                sum += amount;
            }

            if (errors.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error/s occurred.",
                    Details = "You inserted an invalid value/s.",
                    Errors = errors
                };
                return new BadRequestObjectResult(customMessage);
            }

            if (sum != transaction.Amount)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error/s occurred.",
                    Details = "You inserted an invalid value/s.",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The amounts of the splits don't add up to the amount of the transaction"
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
            }


            Transaction transactionSplitted = await _transactionService.ImportSplitsInTransaction(transaction, splits);


            return Ok(transactionSplitted);
        }

        [HttpPost("{id}/categorize")]
        public IActionResult CategorizeTransactions(string id)
        {
            var transactionTask = _transactionService.GetTransactionById(id);
            if (transactionTask.Result.Id == null)
            {
                //if the transaction wasn't found
                var customMessage = new CustomMessage
                {
                    Message = "An error occurred",
                    Details = "No transaction has been found",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "There is no transaction with the id you provided."
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
            }
            var catCode = transactionTask.Result.CatCode;
            var categoryTask = _categoryService.GetCategoryByCode(catCode);
            var category = categoryTask.Result;
            if (category == null)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error occurred",
                    Details = "The transaction has no category",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The transaction's category is null."
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
            }

            return Ok(category);
        }

        [HttpPost("auto-categorize")]
        public async Task<IActionResult> AutoCategorizeTransactions(IFormFile rulesFile)
        {
            if (rulesFile == null || rulesFile.Length == 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "No file has been uploaded.",
                    Details = "You haven't provided a json file."
                };
                return new BadRequestObjectResult(customMessage);
            }
            if (!rulesFile.ContentType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
            {
                var customMessage = new CustomMessage
                {
                    Message = "No json file was uploaded.",
                    Details = "Only json files are allowed for import."
                };
                return new BadRequestObjectResult(customMessage);
            }

            var rulesList = _csvParserService.GetCategoryRules(rulesFile);

            var transactionsWithoutCategory = _transactionService.GetTransactionsWithoutCategories().Result;

            foreach (var transaction in transactionsWithoutCategory)
            {
                if (!string.IsNullOrEmpty(transaction.CatCode))
                {
                    continue;
                }
                foreach (var rule in rulesList)
                {
                    var predicate = BuildPredicate(rule.Predicate);
                    if (predicate(transaction))
                    {
                        transaction.CatCode = rule.CatCode;
                        break;
                    }
                }
            }

            var transactionsWithCategories = transactionsWithoutCategory.Where(x => x.CatCode != null).ToList();

            await _transactionService.UpdateTransactions(transactionsWithCategories);

            var message = new CustomMessage
            {
                Message = "Successful auto-categorization!"
            };
            return new ObjectResult(message);
        }

        private Func<Transaction, bool> BuildPredicate(string predicateExpression)
        {
            // Parse the predicate expression and build a lambda expression
            var parameter = Expression.Parameter(typeof(Transaction), "transaction");
            var lambda = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(bool), predicateExpression);
            return (Func<Transaction, bool>)lambda.Compile();
        }
    }
}
