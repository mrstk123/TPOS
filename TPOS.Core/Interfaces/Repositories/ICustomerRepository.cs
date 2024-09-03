using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities.Generated;

namespace TPOS.Core.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Add customer-specific methods if needed
    }
}
