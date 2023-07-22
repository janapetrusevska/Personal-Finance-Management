using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service.Implementation
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime startDate, DateTime endDate, int page, int pageSize, SortOrder sortOrder, string sortBy)
        {
            var pagedSortedList = await _repository.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);
            return _mapper.Map<PagedSortedList<Transaction>>(pagedSortedList);
        }

        public async Task<PagedSortedList<Transaction>> ImportTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transactionEntities = _mapper.Map<List<TransactionEntity>>(transactions);

            await _repository.ImportTransactions(transactionEntities);

            var pagedSortedList = new PagedSortedList<Transaction>();

            pagedSortedList.TotalCount = transactions.Count;
            pagedSortedList.PageSize = transactions.Count;
            pagedSortedList.Page = 1;
            pagedSortedList.TotalPages = 1;
            pagedSortedList.SortOrder = SortOrder.asc;
            pagedSortedList.SortBy = null;

            pagedSortedList.Items = transactions;

            return _mapper.Map<PagedSortedList<Transaction>>(pagedSortedList);
        }
    }
    
}
