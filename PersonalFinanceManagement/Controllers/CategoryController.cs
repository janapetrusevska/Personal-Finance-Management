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
                    Message="An error occured",
                    Details="You inserted an invalid value",
                    Errors = new List<ErrorDetails>
                {
                    new ErrorDetails
                    {
                        Error = "The value you have provided for parentId is not valid."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }

            return new ObjectResult(categories);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync(IFormFile csvFile)
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
                        Error = "The file you provided is either not valid or empty."
                    }
                }
                };
                return new ObjectResult(customMessage);
            }

            //reading all of the categories from the file
            var categories = _csvParserService.ReadingCategoriesFromFile(csvFile);


            var result = await _categoryService.ImportCategories(categories);
            if (result>0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "Successful import!",
                    Details = result+" categories have been added!",
                };
                return new ObjectResult(customMessage);
            }
            else
            {
                var customMessage = new CustomMessage
                {
                    Message = "Successful import!",
                    Details = "All categories have been updated!",
                };
                return new ObjectResult(customMessage);
            }
        }
    }
}
