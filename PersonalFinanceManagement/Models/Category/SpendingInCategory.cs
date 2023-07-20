using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.Category
{
    public class SpendingInCategory
    {
        public string CatCode { get; set; }
        public int Amount { get; set; }
        public int Count { get; set; }
    }
}
