using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ICsvParserService
    {
        List<Transaction> ReadingTransactionsFromFile(IFormFile csvFile);
    }
}
