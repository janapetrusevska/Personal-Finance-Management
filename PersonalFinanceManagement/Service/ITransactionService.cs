using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ITransactionService
    {
        Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime startDate, DateTime endDate, int page, int pageSize, SortOrder sortOrder, string? sortBy);

        Task<int> ImportTransactions(List<Transaction> transactions);

        Task<Transaction> GetTransactionById(string id);

        Task<TransactionEntity> UpdateCategoryForTransaction(Transaction transaction, Category category);

        CustomMessage areTheDatesInvalid(DateTime startDate, DateTime endDate);
    }
}
