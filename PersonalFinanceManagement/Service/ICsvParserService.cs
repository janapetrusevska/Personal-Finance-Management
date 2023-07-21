﻿using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ICsvParserService
    {
        List<Transaction> ReadingTransactionsFromFile(IFormFile csvFile);

        List<Category> ReadingCategoriesFromFile(IFormFile csvFile);
    }
}
