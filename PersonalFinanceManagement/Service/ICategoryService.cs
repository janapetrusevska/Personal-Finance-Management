using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategories(string parentId);

        Task<Boolean> ImportCategories(List<Category> categories);

        Task<Category> GetCategoryByCode(string code);
    }
}
