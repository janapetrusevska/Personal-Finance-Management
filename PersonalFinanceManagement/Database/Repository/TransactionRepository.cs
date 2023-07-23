using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PfmDbContext _dbContext;

        public TransactionRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionEntity> GetTransactionById(string id)
        {
            var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(x => x.id == id);

            return transaction;
        }

        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string sortBy = null)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount *1.0 / pageSize);

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "id":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.id) : query.OrderByDescending(x => x.id);
                        break;
                    case "cat":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.catCode) : query.OrderByDescending(x => x.catCode);
                        break;
                    case "kind":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.kind) : query.OrderByDescending(x => x.kind);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.direction) : query.OrderByDescending(x => x.direction);
                        break;
                    case "name":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.name) : query.OrderByDescending(x => x.name);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.amount) : query.OrderByDescending(x => x.amount);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.mcc) : query.OrderByDescending(x => x.mcc);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.id);
            }

            if (!String.IsNullOrEmpty(transactionKind))
            {
                if (Enum.TryParse<TransactionKind>(transactionKind, true, out var parsedKind))
                {
                    query = query.Where(x => x.kind == parsedKind);
                }
            }

            if (startDate!=DateTime.MinValue)
            {
                query = query.Where(x => x.date >= startDate.Value);
            }

            if (endDate != DateTime.MinValue)
            {
                query = query.Where(x => x.date <= endDate.Value);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var transactions = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                Page = page,
                TotalPages = totalPages,
                SortOrder = sortOrder,
                SortBy = sortBy,
                Items = transactions
            };
        }

        public async Task ImportTransactions(List<TransactionEntity> transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(transactions);
            await _dbContext.SaveChangesAsync();
            
        }

        public async Task UpdateTransaction(TransactionEntity transactionEntity)
        {
            var existingTransaction = await _dbContext.Transactions.FindAsync(transactionEntity.id);

            if (existingTransaction != null)
            {
                existingTransaction.name = transactionEntity.name;
                existingTransaction.date = transactionEntity.date;
                existingTransaction.direction = transactionEntity.direction;
                existingTransaction.amount = transactionEntity.amount;
                existingTransaction.description = transactionEntity.description;
                existingTransaction.currency = transactionEntity.currency;
                existingTransaction.mcc = transactionEntity.mcc;
                existingTransaction.kind = transactionEntity.kind;
                existingTransaction.catCode = transactionEntity.catCode;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
