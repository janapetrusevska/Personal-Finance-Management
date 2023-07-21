using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ICategoryService
    {
        Task<PagedSortedList<Category>> GetCategories(string parentId);

        Task<List<Category>> ImportCategories(List<Category> categories);
    }
}
