using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManagement.Database.Entities;
using System;

namespace PersonalFinanceManagement.Database.Configuration
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public TransactionEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");
            builder.HasKey(x => x.id); //primary key
            builder.Property(x => x.date).IsRequired();
            builder.Property(x => x.name).HasMaxLength(50);
            builder.Property(x => x.direction).HasConversion<string>().IsRequired().HasMaxLength(1);
            builder.Property(x => x.amount).IsRequired();
            builder.Property(x => x.description).HasMaxLength(255);
            builder.Property(x => x.currency).IsRequired().HasMaxLength(3);
            builder.Property(x => x.mcc).HasConversion<string>().HasMaxLength(4);
            builder.Property(x => x.kind).IsRequired().HasConversion<string>().HasMaxLength(3);

            builder.Property(x => x.catCode);
            //foreign key
            builder.HasOne<CategoryEntity>()
                .WithMany()
                .HasForeignKey(x => x.catCode)
                .HasPrincipalKey(x => x.code);

            builder.HasMany(t => t.splits)
               .WithOne(s => s.transactionEntity)
               .HasForeignKey(s => s.transactionId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
