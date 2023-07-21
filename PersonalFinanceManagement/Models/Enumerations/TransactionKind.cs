using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Models.Enumerations
{
    public enum TransactionKind
    {
        [EnumMember(Value = "dep")]
        dep,
        [EnumMember(Value = "wdw")]
        wdw,
        [EnumMember(Value = "pmt")]
        pmt,
        [EnumMember(Value = "fee")]
        fee,
        [EnumMember(Value = "inc")]
        inc,
        [EnumMember(Value = "rev")]
        rev,
        [EnumMember(Value = "adj")]
        adj,
        [EnumMember(Value = "lnd")]
        lnd,
        [EnumMember(Value = "lnr")]
        lnr,
        [EnumMember(Value = "fcx")]
        fcx,
        [EnumMember(Value = "aop")]
        aop,
        [EnumMember(Value = "acl")]
        acl,
        [EnumMember(Value = "spl")]
        spl,
        [EnumMember(Value = "sal")]
        sal
    }
}
