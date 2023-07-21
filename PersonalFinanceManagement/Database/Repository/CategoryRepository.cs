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

        public Task<PagedSortedList<CategoryEntity>> GetCategories(string parentCode = null)
        {
            throw new NotImplementedException();
        }

        public async Task ImportCategories(List<CategoryEntity> categories)
        {
            await _dbContext.Categories.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }
    }
}
