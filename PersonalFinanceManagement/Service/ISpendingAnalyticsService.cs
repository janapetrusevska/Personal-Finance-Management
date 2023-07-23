﻿using PersonalFinanceManagement.Models.CategoryFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service
{
    public interface ISpendingAnalyticsService
    {
        Task<List<SpendingInCategory>> GetAnalytics(string catCode, DateTime startDate, DateTime endDate, string direction);
    }
}
