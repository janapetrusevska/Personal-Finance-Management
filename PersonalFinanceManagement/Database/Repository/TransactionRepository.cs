using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _dbContext;

        public TransactionRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind = null, string startDate = "1/1/2021", string endDate = "1/1/2021", int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.asc, string sortBy = null)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount *1.0 / pageSize);

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "code":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.id) : query.OrderByDescending(x => x.id);
                        break;
                    case "cat":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.catCode) : query.OrderByDescending(x => x.catCode);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.id);
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
    }
}
