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

        public async Task<Boolean> ImportCategories(List<CategoryEntity> categories)
        {
            bool newCategoriesAdded = false;

            foreach (var category in categories)
            {
                var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.code == category.code);

                if (existingCategory != null)
                {
                    existingCategory = category;
                }
                else
                {
                    _dbContext.Categories.Add(category);
                    newCategoriesAdded = true;
                }
            }
            await _dbContext.SaveChangesAsync();
            return newCategoriesAdded;
        }
    }
}
