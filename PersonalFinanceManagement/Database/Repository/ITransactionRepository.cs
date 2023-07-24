using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public interface ITransactionRepository
    {
        Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string? sortBy = null);
        Task<int> ImportTransactions(List<TransactionEntity> transactions);
        Task<TransactionEntity> GetTransactionById(string id);
        Task UpdateTransaction(TransactionEntity transactionEntity);
        Task<List<TransactionEntity>> GetTransactionsForAnalytics(string catCode = null, DateTime? startDate = null, DateTime? endDate = null, string direction = null);
        CustomMessage CheckForInvalidDates(DateTime? startDate = null, DateTime? endDate = null);
    }
}
