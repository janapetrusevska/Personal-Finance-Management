using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models
{
    public enum Direction
    {
        [EnumMember(Value = "d")]
        d,
        [EnumMember(Value = "c")]
        c
    }
}
