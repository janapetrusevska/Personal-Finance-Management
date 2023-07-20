using PersonalFinanceManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models
{
    public class TransactionWithSplits : Transaction
    {
        public List<Models.Category.SingleCategorySplit> Splits { get; set; }
    }
}
