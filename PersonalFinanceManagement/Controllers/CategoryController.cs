using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Models.Messages;
using PersonalFinanceManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly PfmDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly ICsvParserService _csvParserService;

        public CategoryController(ILogger<CategoryController> logger, PfmDbContext context, ICategoryService categoryService, ICsvParserService csvParserService)
        {
            _logger = logger;
            _context = context;
            _categoryService = categoryService;
            _csvParserService = csvParserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string parentId)
        {
            var categories = await _categoryService.GetCategories(parentId);

            if (categories.Count == 0)
            {
                var customMessage = new CustomMessage
                {
                    Message="An error occurred",
                    Details="You inserted an invalid value",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The value you have provided for parentId is not valid."
                    }
                }
                };
                return new BadRequestObjectResult(customMessage);
            }

            return new ObjectResult(categories);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync(IFormFile csvFile)
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
            

            //reading all of the categories from the file
            var categories = _csvParserService.ReadingCategoriesFromFile(csvFile);


            var result = await _categoryService.ImportCategories(categories);
            var Message = new CustomMessage
            {
                Message = "Successful import!",
                Details = "Number of categories added: " + result[0] + "; Number of categories updated: " + result[1]
            };
            return new ObjectResult(Message);
        }
    }
}
