using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpendingAnalyticsController : ControllerBase
    {
        ITransactionService _transactionService;
        ICategoryService _categoryService;
        ISpendingAnalyticsService _spendingAnalyticsService;
        private readonly ILogger<SpendingAnalyticsController> _logger;
        private readonly PfmDbContext _context;

        public SpendingAnalyticsController(ILogger<SpendingAnalyticsController> logger, PfmDbContext context, ICategoryService categoryService, ITransactionService transactionService, ISpendingAnalyticsService spendingAnalyticsService)
        {
            _logger = logger;
            _context = context;
            _categoryService = categoryService;
            _transactionService = transactionService;
            _spendingAnalyticsService = spendingAnalyticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] string catCode, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string direction)
        {
            var invalidDatesMessage = _transactionService.areTheDatesInvalid(startDate, endDate);
            if (invalidDatesMessage.Message.Count > 0)
            {
                return new ObjectResult(invalidDatesMessage);
            }

            var spendingInAnalytics = await _spendingAnalyticsService.GetAnalytics(catCode,startDate,endDate,direction);

            return Ok(spendingInAnalytics);

        }
    }
}
