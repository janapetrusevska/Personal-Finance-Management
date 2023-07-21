﻿using PersonalFinanceManagement.Database.Entities;
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
        Task ImportCategories(List<CategoryEntity> categories);
    }
}