// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System;
using TPOS.Application.Interfaces;
using TPOS.Application.Interfaces.Repositories;
using TPOS.Infrastructure.Data.Repositories;

namespace TPOS.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
             ProductRepository = new ProductRepository(context);
             DiscountRepository = new DiscountRepository(context);
             UserRepository = new UserRepository(context);
             SupplierRepository = new SupplierRepository(context);
             UserRoleRepository = new UserRoleRepository(context);
             LoyaltyRepository = new LoyaltyRepository(context);
             EmployeeRepository = new EmployeeRepository(context);
             ObjectRepository = new ObjectRepository(context);
             SaleRepository = new SaleRepository(context);
             ContactInfoRepository = new ContactInfoRepository(context);
             CustomerRepository = new CustomerRepository(context);
             PaymentRepository = new PaymentRepository(context);
             CurrencyRateRepository = new CurrencyRateRepository(context);
             SaleItemRepository = new SaleItemRepository(context);
             CategoryRepository = new CategoryRepository(context);
             CompanyRepository = new CompanyRepository(context);
             LoyaltyProgRepository = new LoyaltyProgRepository(context);
             BranchRepository = new BranchRepository(context);
             ProductItemRepository = new ProductItemRepository(context);
             InventoryRepository = new InventoryRepository(context);
             RoleRepository = new RoleRepository(context);
             ProductCategoryRepository = new ProductCategoryRepository(context);
             TaxRepository = new TaxRepository(context);
        }
        public IProductRepository ProductRepository { get; private set; } 

        public IDiscountRepository DiscountRepository { get; private set; } 

        public IUserRepository UserRepository { get; private set; } 

        public ISupplierRepository SupplierRepository { get; private set; } 

        public IUserRoleRepository UserRoleRepository { get; private set; } 

        public ILoyaltyRepository LoyaltyRepository { get; private set; } 

        public IEmployeeRepository EmployeeRepository { get; private set; } 

        public IObjectRepository ObjectRepository { get; private set; } 

        public ISaleRepository SaleRepository { get; private set; } 

        public IContactInfoRepository ContactInfoRepository { get; private set; } 

        public ICustomerRepository CustomerRepository { get; private set; } 

        public IPaymentRepository PaymentRepository { get; private set; } 

        public ICurrencyRateRepository CurrencyRateRepository { get; private set; } 

        public ISaleItemRepository SaleItemRepository { get; private set; } 

        public ICategoryRepository CategoryRepository { get; private set; } 

        public ICompanyRepository CompanyRepository { get; private set; } 

        public ILoyaltyProgRepository LoyaltyProgRepository { get; private set; } 

        public IBranchRepository BranchRepository { get; private set; } 

        public IProductItemRepository ProductItemRepository { get; private set; } 

        public IInventoryRepository InventoryRepository { get; private set; } 

        public IRoleRepository RoleRepository { get; private set; } 

        public IProductCategoryRepository ProductCategoryRepository { get; private set; } 

        public ITaxRepository TaxRepository { get; private set; } 


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}    
