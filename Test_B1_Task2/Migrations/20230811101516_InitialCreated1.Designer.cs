﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Test_B1_Task2.Context;

#nullable disable

namespace Test_B1_Task2.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20230811101516_InitialCreated1")]
    partial class InitialCreated1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Test_B1_Task2.Models.AccountBalance", b =>
                {
                    b.Property<int>("BalanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BalanceID");

                    b.HasIndex("ClassId");

                    b.ToTable("AccountBalance", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.Balance", b =>
                {
                    b.Property<decimal>("OutPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<int>("BalanceID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OutActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnCredit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnDebit")
                        .HasColumnType("TEXT");

                    b.HasKey("OutPassiveSaldo");

                    b.HasIndex("BalanceID")
                        .IsUnique();

                    b.ToTable("Balance", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.Classes", b =>
                {
                    b.Property<int>("ClassID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("STRING");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("STRING");

                    b.HasKey("ClassID");

                    b.ToTable("Classes", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.SumForPart", b =>
                {
                    b.Property<int>("BalanceID")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OutActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OutPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnCredit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnDebit")
                        .HasColumnType("TEXT");

                    b.HasKey("BalanceID");

                    b.ToTable("SumForPart", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.TotalSum", b =>
                {
                    b.Property<int>("TotalSumID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("InActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OutActSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OutPassiveSaldo")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnCredit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TurnDebit")
                        .HasColumnType("TEXT");

                    b.HasKey("TotalSumID");

                    b.ToTable("TotalSum", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.TotalSumInClass", b =>
                {
                    b.Property<int>("ClassID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ClassID");

                    b.ToTable("TotalSumInClass", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.UploadedFiles", b =>
                {
                    b.Property<int>("FileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataLoad")
                        .HasColumnType("DATETIME");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("STRING");

                    b.HasKey("FileID");

                    b.ToTable("UploadedFiles", (string)null);
                });

            modelBuilder.Entity("Test_B1_Task2.Models.AccountBalance", b =>
                {
                    b.HasOne("Test_B1_Task2.Models.Classes", "Classes")
                        .WithMany("AccountBalances")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Test_B1_Task2.Models.Balance", b =>
                {
                    b.HasOne("Test_B1_Task2.Models.AccountBalance", "AccountBalance")
                        .WithOne("Balance")
                        .HasForeignKey("Test_B1_Task2.Models.Balance", "BalanceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountBalance");
                });

            modelBuilder.Entity("Test_B1_Task2.Models.SumForPart", b =>
                {
                    b.HasOne("Test_B1_Task2.Models.AccountBalance", "AccountBalance")
                        .WithOne("SumsForPart")
                        .HasForeignKey("Test_B1_Task2.Models.SumForPart", "BalanceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountBalance");
                });

            modelBuilder.Entity("Test_B1_Task2.Models.TotalSumInClass", b =>
                {
                    b.HasOne("Test_B1_Task2.Models.Classes", "Classes")
                        .WithMany("TotalSumsInClass")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Test_B1_Task2.Models.AccountBalance", b =>
                {
                    b.Navigation("Balance")
                        .IsRequired();

                    b.Navigation("SumsForPart")
                        .IsRequired();
                });

            modelBuilder.Entity("Test_B1_Task2.Models.Classes", b =>
                {
                    b.Navigation("AccountBalances");

                    b.Navigation("TotalSumsInClass");
                });
#pragma warning restore 612, 618
        }
    }
}