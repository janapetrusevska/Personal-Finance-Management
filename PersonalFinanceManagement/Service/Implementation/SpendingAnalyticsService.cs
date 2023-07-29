using AutoMapper;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service.Implementation
{
    public class SpendingAnalyticsService : ISpendingAnalyticsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public SpendingAnalyticsService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<SpendingInCategory>> GetAnalytics(string catCode, string startDate, string endDate, string direction)
        {
            var transactions = await _transactionRepository.GetTransactionsForAnalytics(catCode,startDate,endDate,direction);

            var spendingInCategory = new List<SpendingInCategory>();

            // grouping the transactions so i can do the calculating by group
            var groupedTransactions = transactions.GroupBy(t => t.catCode)
                                                 .Select(g => new SpendingInCategory
                                                 {
                                                     CatCode = g.Key,
                                                     Amount = g.Sum(t => t.amount),
                                                     Count = g.Count()
                                                 })
                                                 .ToList();

            return groupedTransactions;
        }
    }
}
