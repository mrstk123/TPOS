﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using TPOS.Core.Models;
using TPOS.Infrastructure;

namespace TPOS.Infrastructure.Configurations
{
    public partial class LoyaltyProgConfiguration : IEntityTypeConfiguration<LoyaltyProg>
    {
        public void Configure(EntityTypeBuilder<LoyaltyProg> entity)
        {
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LoyaltyProgDescription).HasColumnType("text");
            entity.Property(e => e.LoyaltyProgName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PointsMultiplier).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PointsPerAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Validity).HasColumnType("datetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<LoyaltyProg> entity);
    }
}
