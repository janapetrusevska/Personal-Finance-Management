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
                var messages = new CustomMessage
                {
                    Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 400,
                        Message = "The provided parentId doesn't exist"
                    }
                }
                };
                return new ObjectResult(messages);
            }

            return new ObjectResult(categories);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync(IFormFile csvFile)
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

            //reading all of the categories from the file
            var categories = _csvParserService.ReadingCategoriesFromFile(csvFile);


            var result = await _categoryService.ImportCategories(categories);
            if (result)
            {
                messages.Message = new List<MessageDetails>
                {
                    new MessageDetails
                    {
                        StatusCode = 200,
                        Message = "All new categories have been added!"
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
                        Message = "All categories have been updated!"
                    }
                };
                return new ObjectResult(messages);
            }
        }
    }
}
