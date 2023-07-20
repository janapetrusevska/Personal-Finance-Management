using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ITransactionService
    {
        Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, string startDate, string endDate, int page, int pageSize, SortOrder sortOrder, string? sortBy);

        Task<PagedSortedList<Transaction>> ImportTransactions(List<Transaction> transactions);
    }
}
