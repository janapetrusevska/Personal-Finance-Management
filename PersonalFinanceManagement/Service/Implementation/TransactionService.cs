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

        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<Transaction> GetTransactionById(string id)
        {
            var transactionEntity = await _transactionRepository.GetTransactionById(id);
            var transaction = _mapper.Map<Transaction>(transactionEntity);

            return transaction;
        }

        public async Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime startDate, DateTime endDate, int page, int pageSize, SortOrder sortOrder, string sortBy)
        {
            var pagedSortedList = await _transactionRepository.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortOrder, sortBy);
            return _mapper.Map<PagedSortedList<Transaction>>(pagedSortedList);
        }

        public async Task<PagedSortedList<Transaction>> ImportTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transactionEntities = _mapper.Map<List<TransactionEntity>>(transactions);

            await _transactionRepository.ImportTransactions(transactionEntities);

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

        public async Task<TransactionEntity> UpdateCategoryForTransaction(Transaction transaction, Category category)
        {
            var transactionEntity = await _transactionRepository.GetTransactionById(transaction.Id);
            var categoryEntity = _mapper.Map<CategoryEntity>(category);
            transactionEntity.category = categoryEntity;

            await _transactionRepository.UpdateTransaction(transactionEntity);

            return transactionEntity;
        }
    }
    
}
