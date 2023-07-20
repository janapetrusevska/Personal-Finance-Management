using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.Database.Configuration;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database
{
    public class TransactionDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }

        public TransactionDbContext(DbContextOptions options) : base(options)
        {
        }

        public TransactionDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            modelBuilder.ApplyConfiguration(
                new TransactionEntityTypeConfiguration()
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
