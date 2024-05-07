﻿// <auto-generated />
using System;
using CP.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CP.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CP.Models.Models.InfoAboutCurrency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<double>("AskedCoursePriceTo")
                        .HasColumnType("float");

                    b.Property<int>("AvailOfAskedCourse")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("ID");

                    b.ToTable("InfoAboutCurrency");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            AskedCoursePriceTo = 37.0,
                            AvailOfAskedCourse = 5000,
                            Name = "USD"
                        });
                });

            modelBuilder.Entity("CP.Models.Models.Payments", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DateOfMakingPayments")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sum")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("CP.Models.Models.Purchase", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CurrencyID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfMakingPurchase")
                        .HasColumnType("datetime2");

                    b.Property<double>("DepositedMoney")
                        .HasColumnType("float");

                    b.Property<int>("IDOfUser")
                        .HasColumnType("int");

                    b.Property<double>("MoneyToReturn")
                        .HasColumnType("float");

                    b.Property<double>("SumInUAH")
                        .HasColumnType("float");

                    b.Property<double>("SumOfCurrency")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CurrencyID");

                    b.HasIndex("IDOfUser");

                    b.ToTable("Purchase");
                });

            modelBuilder.Entity("CP.Models.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CP.Models.Models.Purchase", b =>
                {
                    b.HasOne("CP.Models.Models.InfoAboutCurrency", "InfoAboutCurrency")
                        .WithMany()
                        .HasForeignKey("CurrencyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CP.Models.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("IDOfUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InfoAboutCurrency");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
