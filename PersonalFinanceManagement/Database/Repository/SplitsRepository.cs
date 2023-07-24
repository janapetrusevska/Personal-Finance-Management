using PersonalFinanceManagement.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Repository
{
    public class SplitsRepository : ISplitsRepository
    {
        private readonly PfmDbContext _dbContext;

        public SplitsRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ImportSplits(List<SplitsEntity> splits)
        {
            _dbContext.Splits.AddRange(splits);
            await _dbContext.SaveChangesAsync();
        }
    }
}
