using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Entities
{
    public class SplitsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string catCode { get; set; }
        public double amount { get; set; }
        //public string transactionId { get; set; }
        //[ForeignKey("transactionId")]
        //[JsonIgnore]
        //public TransactionEntity transaction { get; set; }
    }
}
