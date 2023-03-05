﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicSchool.Finance.Infrastructure.DataAccess;

#nullable disable

namespace MusicSchool.Finance.Api.Migrations
{
    [DbContext(typeof(FinanceContext))]
    partial class FinanceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("char(7)")
                        .HasColumnName("month");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)")
                        .HasColumnName("student_id");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("student_name");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("total_value");

                    b.HasKey("Id");

                    b.ToTable("invoices", (string)null);
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.InvoiceItem", b =>
                {
                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("char(36)")
                        .HasColumnName("invoice_id");

                    b.Property<Guid>("EnrollmentId")
                        .HasColumnType("char(36)")
                        .HasColumnName("enrollment_id");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("char(36)")
                        .HasColumnName("course_id");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("course_name");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("value");

                    b.HasKey("InvoiceId", "EnrollmentId");

                    b.ToTable("invoice_items", (string)null);
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("char(50)")
                        .HasColumnName("type");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.ToTable("transactions", (string)null);

                    b.HasDiscriminator<string>("Type").HasValue("Transaction");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.InvoicePayment", b =>
                {
                    b.HasBaseType("MusicSchool.Finance.Domain.Entities.Transaction");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("char(36)")
                        .HasColumnName("invoice_id");

                    b.HasIndex("InvoiceId");

                    b.HasDiscriminator().HasValue("invoice_payment");
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.InvoiceItem", b =>
                {
                    b.HasOne("MusicSchool.Finance.Domain.Entities.Invoice", null)
                        .WithMany("Items")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.InvoicePayment", b =>
                {
                    b.HasOne("MusicSchool.Finance.Domain.Entities.Invoice", null)
                        .WithMany()
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicSchool.Finance.Domain.Entities.Invoice", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
