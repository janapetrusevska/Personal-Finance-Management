﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PersonalFinanceManagement.Database;

namespace PersonalFinanceManagement.Migrations
{
    [DbContext(typeof(PfmDbContext))]
    partial class TransactionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("PersonalFinanceManagement.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("code")
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("parentCode")
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.HasKey("code");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("PersonalFinanceManagement.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<double>("amount")
                        .HasColumnType("double precision");

                    b.Property<string>("catCode")
                        .HasColumnType("text");

                    b.Property<string>("categorycode")
                        .HasColumnType("text");

                    b.Property<string>("currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("direction")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("character varying(1)");

                    b.Property<string>("kind")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<string>("mcc")
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("id");

                    b.HasIndex("catCode");

                    b.HasIndex("categorycode");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("PersonalFinanceManagement.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("PersonalFinanceManagement.Database.Entities.CategoryEntity", null)
                        .WithMany()
                        .HasForeignKey("catCode");

                    b.HasOne("PersonalFinanceManagement.Database.Entities.CategoryEntity", "category")
                        .WithMany()
                        .HasForeignKey("categorycode");

                    b.Navigation("category");
                });
#pragma warning restore 612, 618
        }
    }
}
