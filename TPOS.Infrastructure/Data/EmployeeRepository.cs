using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Infrastructure.Data
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        // Implement employee-specific methods if needed
    }
}
