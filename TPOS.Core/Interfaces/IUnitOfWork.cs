using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IBranchRepository BranchRepository { get; }
        IContactInfoRepository ContactInfoRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }

        Task<int> CompleteAsync();

    }
}
