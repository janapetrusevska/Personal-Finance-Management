using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManagement.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Database.Configuration
{
    public class SplitsEntityTypeConfiguration : IEntityTypeConfiguration<SplitsEntity>
    {
        public SplitsEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<SplitsEntity> builder)
        {
            builder.ToTable("splits");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.catCode).IsRequired().HasMaxLength(1);
            builder.Property(x => x.amount).IsRequired();
            builder.Property(x => x.transactionId);
        }
    }
}
