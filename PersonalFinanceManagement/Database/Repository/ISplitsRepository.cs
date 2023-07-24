using PersonalFinanceManagement.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public interface ISplitsRepository
    {
        Task ImportSplits(List<SplitsEntity> splits);
    }
}
