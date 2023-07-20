using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Mappings;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        private readonly TransactionDbContext _transactionContext;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, TransactionDbContext transactionContext)
        {
            _logger = logger;
            _transactionService = transactionService;
            _transactionContext = transactionContext;
        }

        [HttpGet]
        public IActionResult GetTransactions([FromQuery] string transactionKind, [FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.asc, [FromQuery] string? sortBy = null)
        {
            var transactions = _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);
            return Ok(transactions);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                return BadRequest("No file uploaded");
            } 

            List<Transaction> transactions = new List<Transaction>();

            using (var stream = csvFile.OpenReadStream())
            {
                using(var reader = new StreamReader(stream))
                {
                    CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null,
                        TrimOptions = TrimOptions.Trim,
                    };
                    CsvReader csv = new CsvReader(reader, csvConfig);
                    csv.Context.RegisterClassMap<TransactionMap>();

                    List<Transaction> csvTransactions = csv.GetRecords<Transaction>().ToList();
                }

                foreach (Transaction t in transactions)
                {
                    if (t.Id != null)
                    {
                        //error handling
                        continue;
                    }
                }
                var result = await _transactionService.ImportTransactions(transactions);
            }

            return Ok("CSV file imported successfully");
        }

        [HttpPost("{id}/split")]
        public IActionResult SplitTransactions()
        {
            return Ok();
        }

        [HttpPost("{id}/categorize")]
        public IActionResult CategorizeTransactions()
        {
            return Ok();
        }

        [HttpPost("auto-categorize")]
        public IActionResult AutoCategorizeTransactions()
        {
            return Ok();
        }
    }
}
