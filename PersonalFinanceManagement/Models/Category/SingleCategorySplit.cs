using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.Category
{
    public class SingleCategorySplit
    {
        [Required]
        public string CateCode { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
