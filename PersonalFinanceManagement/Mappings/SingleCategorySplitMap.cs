using CsvHelper.Configuration;
using PersonalFinanceManagement.Models.CategoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Mappings
{
    public class SingleCategorySplitMap : ClassMap<SingleCategorySplit>
    {
        public SingleCategorySplitMap()
        {
            Map(m => m.Amount).Name("amount");
            Map(m => m.CatCode).Name("catCode");
        }
    }
}
