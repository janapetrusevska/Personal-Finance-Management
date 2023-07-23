using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonalFinanceManagement.Database;
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

            return Ok(categories);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportCategoriesAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            //reading all of the categories from the file
            var categories = _csvParserService.ReadingCategoriesFromFile(csvFile);


            var result = await _categoryService.ImportCategories(categories);
            if (result)
            {
                return Ok("All new categories were added!");
            }
            else
            {
                return Ok("All categories have been updated");
            }
        }
    }
}
