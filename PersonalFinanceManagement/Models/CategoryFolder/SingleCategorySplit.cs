using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.CategoryFolder
{
    public class SingleCategorySplit
    {
        [Required]
        public string CatCode { get; set; }
        [Required]
        public double Amount { get; set; }
        public string TransactionId { get; set; }
    }
}
