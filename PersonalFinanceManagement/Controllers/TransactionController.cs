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
        private readonly ILogger<TransactionController> _logger;
        private readonly PfmDbContext _transactionContext;
        private readonly ICsvParserService _csvParserService;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, PfmDbContext transactionContext, ICsvParserService csvParserService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _transactionContext = transactionContext;
            _csvParserService = csvParserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] string transactionKind, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.asc, [FromQuery] string? sortBy = null)
        {
            var transactions = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);

            return Ok(transactions);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportTransactionsAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            if (_transactionContext.Transactions.Any())
            {
                return Ok("The database is already filled");
            }

            var transactions = _csvParserService.ReadingTransactionsFromFile(csvFile);

            var result = await _transactionService.ImportTransactions(transactions);

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
