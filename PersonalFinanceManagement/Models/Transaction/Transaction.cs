using PersonalFinanceManagement.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models
{
    public class Transaction
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string BeneficiaryName { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        public string Currency { get; set; }
        public string MccCode { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; } //CategoryCode
    }
}
