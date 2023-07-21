using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Entities
{
    public class CategoryEntity
    {
        public string code { get; set; }
        public string name { get; set; }
        public string parentCode { get; set; }
    }
}
