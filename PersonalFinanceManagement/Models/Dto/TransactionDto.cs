using PersonalFinanceManagement.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.Dto
{
    public class TransactionDto
    { 
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DateTime Date { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string MccCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; }
        public List<SplitDto> Splits { get; set; }
    }
}
