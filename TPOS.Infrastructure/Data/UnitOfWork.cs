using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Interfaces.Repositories;
using TPOS.Core.Interfaces;
using TPOS.Infrastructure.Data.Repositories;

namespace TPOS.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ContactInfoRepository = new ContactInfoRepository(context);
            CustomerRepository = new CustomerRepository(context);
            EmployeeRepository = new EmployeeRepository(context);
            UserRepository = new UserRepository(context);
            RoleRepository = new RoleRepository(context);
            UserRoleRepository = new UserRoleRepository(context);
        }

        public IContactInfoRepository ContactInfoRepository { get; private set; }
        public ICustomerRepository CustomerRepository { get; private set; }
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IUserRoleRepository UserRoleRepository { get; private set; }


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
