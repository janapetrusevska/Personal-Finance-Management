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
        CustomMessage CheckForInvalidDates(string? startDate = null, string? endDate = null);
        Task<List<int>> ImportTransactions(List<TransactionEntity> transactions);
        Task<TransactionEntity> AddTransaction(TransactionEntity transactionEntity);
        Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, string? startDate = null, string? endDate = null, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string? sortBy = null);
        Task<TransactionEntity> GetTransactionById(string id);
        Task<List<TransactionEntity>> GetTransactionsForAnalytics(string catCode = null, string? startDate = null, string? endDate = null, string direction = null);
        Task UpdateTransactionsSplits(TransactionEntity transactionEntity);
        Task<List<TransactionEntity>> GetTransactionsWithoutCategories();
        Task UpdateTransactions(List<TransactionEntity> transactionEntities);
    }
}
