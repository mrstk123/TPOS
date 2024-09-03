using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities;

namespace TPOS.Core.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // Add employee-specific methods if needed
    }
}
