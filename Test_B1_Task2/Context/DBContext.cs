using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_B1_Task2.Models;

namespace Test_B1_Task2.Context
{
    internal class DBContext : DbContext
    {

        string connectionString = "Data Source=G:/testasd.db";
        public DbSet<AccountBalance> AccountBalances { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Classes> Classess { get; set; }
        public DbSet<SumForPart> SumForParts { get; set; }
        public DbSet<TotalSum> TotalSums { get; set; }
        public DbSet<TotalSumInClass> TotalSumInClasses { get; set; }
        public DbSet<UploadedFiles> UploadedFiless { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(connectionString);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.ToTable("Classes");
                entity.HasKey(e => e.ClassID);
                entity.Property(e => e.ClassID).IsRequired();
                entity.Property(e => e.ClassName).HasColumnType("STRING");
                entity.Property(e => e.FileName).HasColumnType("STRING");
            });

            modelBuilder.Entity<AccountBalance>(entity =>
            {
                entity.ToTable("AccountBalance");
                entity.HasKey(e => e.BalanceID);
                entity.Property(e => e.BalanceID).IsRequired();
                entity.Property(e => e.ClassId).IsRequired();

                entity.HasOne(d => d.Classes)
                    .WithMany(p => p.AccountBalances)
                    .HasForeignKey(d => d.ClassId);
            });

            modelBuilder.Entity<Balance>(entity =>
            {
                entity.ToTable("Balance");
                entity.HasKey(e => e.BalanceID);
                entity.HasKey(e => e.InActSaldo);
                entity.HasKey(e => e.InPassiveSaldo);
                entity.HasKey(e => e.TurnDebit);
                entity.HasKey(e => e.TurnCredit);
                entity.HasKey(e => e.OutActSaldo);
                entity.HasKey(e => e.OutPassiveSaldo);
                entity.Property(e => e.BalanceID).IsRequired();
                entity.HasOne(d => d.AccountBalance)
                    .WithOne(p => p.Balance)
                    .HasForeignKey<Balance>(d => d.BalanceID);
            });

            modelBuilder.Entity<SumForPart>(entity =>
            {
                entity.ToTable("SumForPart");
                entity.HasKey(e => e.BalanceID);
                entity.Property(e => e.BalanceID).IsRequired();
                entity.HasOne(d => d.AccountBalance)
                    .WithOne(p => p.SumsForPart)
                    .HasForeignKey<SumForPart>(d => d.BalanceID);
            });

            modelBuilder.Entity<TotalSum>(entity =>
            {
                entity.ToTable("TotalSum");
                entity.HasKey(e => e.InPassiveSaldo);
                entity.HasKey(e => e.OutPassiveSaldo);
                entity.HasKey(e => e.InActSaldo);
                entity.HasKey(e => e.TurnCredit);
                entity.HasKey(e => e.TurnDebit);
                entity.HasKey(e => e.OutActSaldo);
                entity.HasKey(e => e.TotalSumID);
                entity.Property(e => e.TotalSumID)
                            .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TotalSumInClass>(entity =>
            {
                entity.ToTable("TotalSumInClass");
                entity.HasKey(e => new { e.ClassID });
                entity.Property(e => e.ClassID).IsRequired();
                entity.HasOne(d => d.Classes)
                    .WithMany(p => p.TotalSumsInClass)
                    .HasForeignKey(d => d.ClassID);
            });

            modelBuilder.Entity<UploadedFiles>(entity =>
            {
                entity.ToTable("UploadedFiles");
                entity.HasKey(e => e.FileID);
                entity.Property(e => e.FileID).ValueGeneratedOnAdd();
                entity.Property(e => e.FileName).HasColumnType("STRING");
                entity.Property(e => e.DataLoad).HasColumnType("DATETIME");
            });
        }


    }
}
