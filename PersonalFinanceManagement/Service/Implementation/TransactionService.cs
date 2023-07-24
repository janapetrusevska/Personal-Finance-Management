using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceManagement.Database;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using PersonalFinanceManagement.Models.Dto;
using PersonalFinanceManagement.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service.Implementation
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly ISplitsRepository _splitRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ISplitsRepository splitsRepository)
        {
            _transactionRepository = transactionRepository;
            _splitRepository = splitsRepository;
            _mapper = mapper;
        }

        public CustomMessage areTheDatesInvalid(DateTime startDate, DateTime endDate)
        {
            var result =  _transactionRepository.CheckForInvalidDates(startDate, endDate);
            return result;
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

        public async Task<int> ImportTransactions(List<Transaction> transactions)
        {
            List<TransactionEntity> transactionEntities = _mapper.Map<List<TransactionEntity>>(transactions);

            var result = await _transactionRepository.ImportTransactions(transactionEntities);

            return result;
        }

        public async Task<Transaction> ImportSplitsInTransaction(Transaction transaction, List<SingleCategorySplit> splits)
        {
            transaction.Splits.Clear();

            foreach (var splitRequest in splits)
            {
                var split = new SingleCategorySplit
                {
                    Amount = splitRequest.Amount,
                    CatCode = splitRequest.CatCode
                };
                transaction.Splits.Add(split);
            }
            var transactionEntity = _mapper.Map<TransactionEntity>(transaction);

            //updating the transaction's splits
            await _transactionRepository.UpdateTransactionsSplits(transactionEntity);

            //adding the splits

            var splitsEntities = _mapper.Map<List<SplitsEntity>>(splits);
            await _splitRepository.ImportSplits(splitsEntities);

            transaction = _mapper.Map<Transaction>(transactionEntity);
            return transaction;
        }
    }
    
}
