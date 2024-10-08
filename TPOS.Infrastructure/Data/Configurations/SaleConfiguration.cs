﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TPOS.Domain.Entities.Generated;
using TPOS.Infrastructure.Data;

namespace TPOS.Infrastructure.Data.Configurations
{
    public partial class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> entity)
        {
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NetAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ReceiptNumber)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BranchID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Branches");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerID)
                .HasConstraintName("FK_Sales_Customers");

            entity.HasOne(d => d.Discount).WithMany(p => p.Sales)
                .HasForeignKey(d => d.DiscountID)
                .HasConstraintName("FK_Sales_Discounts");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Employees");

            entity.HasOne(d => d.SaleCurrency).WithMany(p => p.SaleSaleCurrencies)
                .HasForeignKey(d => d.SaleCurrencyID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Objects");

            entity.HasOne(d => d.Status).WithMany(p => p.SaleStatuses)
                .HasForeignKey(d => d.StatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Objects1");

            entity.HasOne(d => d.Tax).WithMany(p => p.Sales)
                .HasForeignKey(d => d.TaxID)
                .HasConstraintName("FK_Sales_Taxes");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Sale> entity);
    }
}
