using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly PfmDbContext _dbContext;

        public CategoryRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CategoryEntity>> GetAllCategories()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories;
        }

        public async Task<List<CategoryEntity>> GetCategories(string parentCode = null)
        {
            var query = _dbContext.Categories.AsQueryable();

            if (!String.IsNullOrEmpty(parentCode))
            {
                query = query.Where(c => c.parentCode == parentCode);
            }
            else
            {
                query = query.Where(c => c.parentCode == "");
            }
            var categories = await query.ToListAsync();

            return categories;
        }

        public async Task<CategoryEntity> GetCategoryByCode(string code)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(x => x.code == code);

            return category;
        }

        public async Task<List<int>> ImportCategories(List<CategoryEntity> categories)
        {
            List<int> count = new List<int>();
            var updated = 0;
            var added = 0;

            foreach (var category in categories)
            {
                var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.code == category.code);

                if (existingCategory != null)
                {
                    if(existingCategory.name != category.name)
                        existingCategory.name = category.name;
                    if (existingCategory.code != category.code)
                        existingCategory.code = category.code;
                    if (existingCategory.parentCode != category.parentCode)
                        existingCategory.parentCode = category.parentCode;
                    updated++;
                }
                else
                {
                    _dbContext.Categories.Add(category);
                    added++;
                }
            }
            await _dbContext.SaveChangesAsync();
            count.Add(added);
            count.Add(updated);
            return count;
        }
    }
}
