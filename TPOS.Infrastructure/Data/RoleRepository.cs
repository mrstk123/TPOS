using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Infrastructure.Data
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        // Implement user-specific methods if needed
    }
}
