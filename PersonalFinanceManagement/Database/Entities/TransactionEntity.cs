using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Entities
{
    public class TransactionEntity
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public Direction direction { get; set; }
        public double amount { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string mcc { get; set; }
        public TransactionKind kind { get; set; }
        public string catCode { get; set; }
        [ForeignKey("catCode")]
        public CategoryEntity category{ get; set; }
    }
}
