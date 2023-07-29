using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Enumerations;
using PersonalFinanceManagement.Models.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public CustomMessage CheckForInvalidDates(string? startDate = null, string? endDate = null)
        {
            var errors = new List<ErrorDetails>();

            if (!string.IsNullOrEmpty(startDate) && !DateTime.TryParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                errors.Add(new ErrorDetails
                {
                    Error = "startDate must be a valid date in the format 'dd-MM-yyyy'."
                });
            }

            if (!string.IsNullOrEmpty(endDate) && !DateTime.TryParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                errors.Add(new ErrorDetails
                {
                    Error = "endDate must be a valid date in the format 'dd-MM-yyyy'."
                });
            }

            if (errors.Count > 0)
            {
                var customMessage = new CustomMessage
                {
                    Message = "An error(s) occurred.",
                    Details = "You inserted an invalid value(s).",
                    Errors = errors
                };
                return customMessage;
            }
            else
            {
                var customMessage = new CustomMessage
                {
                    Errors = new List<ErrorDetails>()
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

        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, string? startDate = null, string? endDate = null, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string sortBy = null)
        {

            if (pageSize > 100)
            {
                pageSize = 100;
            }
            var query = _dbContext.Transactions.AsQueryable();

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
                query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.id) : query.OrderByDescending(x => x.id);
            }

            if (!String.IsNullOrEmpty(transactionKind))
            {
                if (Enum.TryParse<TransactionKind>(transactionKind, true, out var parsedKind))
                {
                    query = query.Where(x => x.kind == parsedKind);
                }
            }

            string format = "dd-MM-yyyy";
            if (startDate != null)
            {
                DateTime parsedStartDate = DateTime.ParseExact(startDate, format, System.Globalization.CultureInfo.InvariantCulture);
                if (parsedStartDate != DateTime.MinValue)
                {
                    query = query.Where(x => x.date >= parsedStartDate);
                }
            }
            if (endDate != null)
            {
                DateTime parsedEndDate = DateTime.ParseExact(endDate, format, System.Globalization.CultureInfo.InvariantCulture);
                if (parsedEndDate != DateTime.MinValue)
                {
                    query = query.Where(x => x.date <= parsedEndDate);
                }
            }

            

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

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

        public async Task<List<TransactionEntity>> GetTransactionsForAnalytics(string catCode = null, string? startDate = null, string? endDate = null, string direction = null)
        {
            List<TransactionEntity> transactions = new List<TransactionEntity>();
            if (catCode != null)
            {
                 transactions = await _dbContext.Transactions.Where(x => x.catCode == catCode).ToListAsync();
            }
            else
            {
                 transactions = await _dbContext.Transactions.Where(x => x.catCode != null).ToListAsync();
            }

            var query = transactions.AsQueryable();
            query = query.OrderBy(x => x.id);

            string format = "dd-MM-yyyy";
            if (startDate != null)
            {
                DateTime parsedStartDate = DateTime.ParseExact(startDate, format, System.Globalization.CultureInfo.InvariantCulture);
                if (parsedStartDate != DateTime.MinValue)
                {
                    query = query.Where(x => x.date >= parsedStartDate);
                }
            }
            if (endDate != null)
            {
                DateTime parsedEndDate = DateTime.ParseExact(endDate, format, System.Globalization.CultureInfo.InvariantCulture);
                if (parsedEndDate != DateTime.MinValue)
                {
                    query = query.Where(x => x.date <= parsedEndDate);
                }
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
        public async Task<List<int>> ImportTransactions(List<TransactionEntity> transactions)
        {
            List<int> count = new List<int>();
            var updated = 0;
            var added = 0;

            foreach (var transaction in transactions)
            {
                var existingTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.id == transaction.id);
                if (existingTransaction != null)
                {
                    if (existingTransaction.name != transaction.name)
                        existingTransaction.name = transaction.name;

                    if (existingTransaction.direction != transaction.direction)
                        existingTransaction.direction = transaction.direction;

                    if (existingTransaction.amount != transaction.amount)
                        existingTransaction.amount = transaction.amount;

                    if (existingTransaction.description != transaction.description)
                        existingTransaction.description = transaction.description;

                    if (existingTransaction.currency != transaction.currency)
                        existingTransaction.currency = transaction.currency;

                    if (existingTransaction.mcc != transaction.mcc)
                        existingTransaction.mcc = transaction.mcc;

                    if (existingTransaction.kind != transaction.kind)
                        existingTransaction.kind = transaction.kind;

                    if (existingTransaction.catCode != transaction.catCode)
                        existingTransaction.catCode = transaction.catCode;

                    if (existingTransaction.date != transaction.date)
                        existingTransaction.date = transaction.date;
                    updated++;
                }
                else
                {
                    _dbContext.Transactions.Add(transaction);
                    added++;
                }
            }
            await _dbContext.SaveChangesAsync();
            count.Add(added);
            count.Add(updated);
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
                //await _dbContext.SaveChangesAsync();
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TransactionEntity>> GetTransactionsWithoutCategories()
        {
            var transactions = await _dbContext.Transactions.Where(t => t.catCode == null).ToListAsync();
            return transactions;
        }

        public async Task UpdateTransactions(List<TransactionEntity> transactionEntities)
        {
            foreach(TransactionEntity transaction in transactionEntities)
            {
                if(transaction.catCode != null)
                {
                    var existingTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.id == transaction.id);
                    if (existingTransaction != null)
                    {
                        existingTransaction.catCode = transaction.catCode;
                        _dbContext.Transactions.Update(existingTransaction);
                    }                
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
