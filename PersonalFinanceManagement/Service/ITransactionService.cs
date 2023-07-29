using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ITransactionService
    {
        Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, string? startDate, string? endDate, int page, int pageSize, SortOrder sortOrder, string? sortBy);

        Task<List<int>> ImportTransactions(List<Transaction> transactions);

        Task<Transaction> GetTransactionById(string id);

        CustomMessage areTheDatesInvalid(string startDate, string endDate);

        Task<Transaction> ImportSplitsInTransaction(Transaction transaction, List<SingleCategorySplit> splits);

        Task<List<Transaction>> GetTransactionsWithoutCategories();

        Task UpdateTransactions(List<Transaction> transactionsWithoutCategory);
    }
}
