using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Enumerations;
using PersonalFinanceManagement.Models.Messages;
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

        public CustomMessage CheckForInvalidDates(DateTime? startDate = null, DateTime? endDate = null)
        { 
            var messages = new List<MessageDetails>();

            if (startDate.HasValue && startDate.Value!=DateTime.MinValue && startDate.Value.Kind != DateTimeKind.Utc)
            {
                messages.Add(new MessageDetails
                {
                    StatusCode = 400,
                    Message = "startDate must be a valid DateTime in UTC format."
                });
            }
            if (endDate.HasValue && endDate.Value!=DateTime.MinValue && endDate.Value.Kind != DateTimeKind.Utc)
            {
                messages.Add(new MessageDetails
                {
                    StatusCode = 400,
                    Message = "endDate must be a valid DateTime in UTC format."
                });
            }
            if (messages.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = messages
                };
                return customMessage;
            }
            else
            {
                var customMessage = new CustomMessage
                {
                    Message = new List<MessageDetails>()
                };
                return customMessage;
            }
        }

        public async Task<TransactionEntity> GetTransactionById(string id)
        {
            var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(x => x.id == id);
            if (transaction == null)
            {
                //when it can't find a transaction with that id we send an empty one
                return new TransactionEntity();
            }

            return transaction;
        }

        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string sortBy = null)
        {

            if (pageSize > 100)
            {
                pageSize = 100;
            }
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

        public async Task<List<TransactionEntity>> GetTransactionsForAnalytics(string catCode = null, DateTime? startDate = null, DateTime? endDate = null, string direction = null)
        {
            List<TransactionEntity> transactions = new List<TransactionEntity>();
            if (catCode != null)
            {
                 transactions = await _dbContext.Transactions.Where(x => x.catCode == catCode).ToListAsync();
            }
            else
            {
                 transactions = await _dbContext.Transactions.ToListAsync();
            }
            
            var query = transactions.AsQueryable();
            query = query.OrderBy(x => x.id);

            if (startDate != DateTime.MinValue)
            {
                query = query.Where(x => x.date >= startDate.Value);
            }

            if (endDate != DateTime.MinValue)
            {
                query = query.Where(x => x.date <= endDate.Value);
            }

            if (!String.IsNullOrEmpty(direction))
            {
                if (Enum.TryParse<Direction>(direction, true, out var parsedDirection))
                {
                    query = query.Where(x => x.direction == parsedDirection);
                }
            }

            transactions = query.ToList();

            return transactions;
        }
        public async Task<int> ImportTransactions(List<TransactionEntity> transactions)
        {
            var count = 0;

            foreach (var transaction in transactions)
            {
                var existingTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.id == transaction.id);

                if (existingTransaction != null)
                {
                    existingTransaction = transaction;
                }
                else
                {
                    _dbContext.Transactions.Add(transaction);
                    count++;
                }
            }
            await _dbContext.SaveChangesAsync();
            return count;

        }

        public async Task<TransactionEntity> AddTransaction(TransactionEntity transactionEntity)
        {
            _dbContext.Transactions.Add(transactionEntity);
            await _dbContext.SaveChangesAsync();
            return transactionEntity;
        }

        public async Task UpdateTransactionsSplits(TransactionEntity transactionEntity)
        {
            var existingTransaction = await _dbContext.Transactions.Include(t => t.splits).FirstOrDefaultAsync(t => t.id == transactionEntity.id);

            if (existingTransaction != null)
            {
                existingTransaction.splits = transactionEntity.splits;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
