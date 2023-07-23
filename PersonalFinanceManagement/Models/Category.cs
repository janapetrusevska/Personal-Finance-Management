using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models
{
    public class Category
    {
        [Required]
        public string Code { get; set; }
        [Required] 
        public string Name { get; set; }
        public string ParentCode { get; set; }
    }
}
