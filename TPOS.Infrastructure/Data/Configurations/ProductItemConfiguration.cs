﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TPOS.Domain.Entities.Generated;
using TPOS.Infrastructure.Data;

namespace TPOS.Infrastructure.Data.Configurations
{
    public partial class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
    {
        public void Configure(EntityTypeBuilder<ProductItem> entity)
        {
            entity.HasIndex(e => e.SerialNumber, "IX_ProductItems").IsUnique();

            entity.Property(e => e.BatchNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.SaleDate).HasColumnType("datetime");
            entity.Property(e => e.SerialNumber)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.BranchID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductItems_Branches");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductItems_Products");

            entity.HasOne(d => d.Status).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.StatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductItems_Objects");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ProductItem> entity);
    }
}
