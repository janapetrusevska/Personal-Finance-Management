using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetCategories(string parentCode = null);
        Task<List<int>> ImportCategories(List<CategoryEntity> categories);
        Task<CategoryEntity> GetCategoryByCode(string code);

        Task<List<CategoryEntity>> GetAllCategories();
    }
}
