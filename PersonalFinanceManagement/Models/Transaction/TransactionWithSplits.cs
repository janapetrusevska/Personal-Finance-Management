using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.CategoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models
{
    public class TransactionWithSplits : Transaction
    {
        public List<SingleCategorySplit> Splits { get; set; }
    }
}
