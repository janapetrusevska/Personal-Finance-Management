using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.Messages
{
    public class CustomMessage
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public List<ErrorDetails> Errors { get; set; }
    }
}
