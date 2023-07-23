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
    public class PfmDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }


        public PfmDbContext(DbContextOptions options) : base(options)
        {
        }

        public PfmDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            modelBuilder.ApplyConfiguration(
                new TransactionEntityTypeConfiguration()
                );
            modelBuilder.ApplyConfiguration(
                new CategoryEntityTypeConfiguration()
                );
            base.OnModelCreating(modelBuilder);
        }

    }
}
