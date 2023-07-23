using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Mappings;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service.Implementation
{
    public class CsvParserService : ICsvParserService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CsvParserService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public List<Transaction> ReadingTransactionsFromFile(IFormFile csvFile)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (var stream = csvFile.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null,
                        TrimOptions = TrimOptions.Trim,
                    };
                    CsvReader csv = new CsvReader(reader, csvConfig);
                    csv.Context.RegisterClassMap<TransactionMap>();

                    transactions = csv.GetRecords<Transaction>().ToList();
                }


                //foreach (Transaction t in transactions)
                //{
                //    if (t.Id != null)
                //    {
                //        //error handling
                //        continue;
                //    }
                //}
                return transactions;
            }

        }

        public List<Category> ReadingCategoriesFromFile(IFormFile csvFile)
        {
            List<Category> categories = new List<Category>();

            using (var stream = csvFile.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        MissingFieldFound = null,
                        TrimOptions = TrimOptions.Trim,
                    };
                    CsvReader csv = new CsvReader(reader, csvConfig);
                    csv.Context.RegisterClassMap<CategoryMap>();

                    categories = csv.GetRecords<Category>().ToList();
                }

                return categories;
            }

        }
    }
}
