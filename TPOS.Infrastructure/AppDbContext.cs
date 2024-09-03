﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TPOS.Infrastructure.Configurations;
using TPOS.Core.Entities.Generated;
using Object = TPOS.Core.Entities.Generated.Object;
#nullable disable

namespace TPOS.Infrastructure;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<ContactInfo> ContactInfos { get; set; }

    public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Loyalty> Loyalties { get; set; }

    public virtual DbSet<LoyaltyProg> LoyaltyProgs { get; set; }

    public virtual DbSet<Object> Objects { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleItem> SaleItems { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.BranchConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ContactInfoConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CurrencyRateConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.InventoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.LoyaltyConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.LoyaltyProgConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ObjectConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ProductConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ProductCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.ProductItemConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.RoleConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SaleConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SaleItemConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.TaxConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
        modelBuilder.ApplyConfiguration(new Configurations.UserRoleConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
