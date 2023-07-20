using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public interface ITransactionRepository
    {
        Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, string startDate = "1/1/2021", string endDate = "1/1/2021", int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string? sortBy = null);
        Task ImportTransactions(List<TransactionEntity> transactions);
    }
}
