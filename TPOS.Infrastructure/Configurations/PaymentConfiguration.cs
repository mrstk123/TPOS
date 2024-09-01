﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TPOS.Core.Models;
using TPOS.Infrastructure;

namespace TPOS.Infrastructure.Configurations
{
    public partial class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> entity)
        {
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.PaymentCurrency).WithMany(p => p.PaymentPaymentCurrencies)
                .HasForeignKey(d => d.PaymentCurrencyID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Objects");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.PaymentPaymentTypes)
                .HasForeignKey(d => d.PaymentTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Objects1");

            entity.HasOne(d => d.Sale).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SaleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Sales");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Payment> entity);
    }
}
