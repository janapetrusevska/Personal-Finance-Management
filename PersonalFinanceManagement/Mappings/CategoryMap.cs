using CsvHelper.Configuration;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Mappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Map(m => m.Name).Name("name");
            Map(m => m.Code).Name("code");
            Map(m => m.ParentCode).Name("parent-code");
        }
    }
}
