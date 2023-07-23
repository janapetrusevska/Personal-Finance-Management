using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.CategoryFolder
{
    public class SpendingInCategory
    {
        [ForeignKey("CatCode")]
        public Category Category { get; set; }
        public string CatCode { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
    }
}
