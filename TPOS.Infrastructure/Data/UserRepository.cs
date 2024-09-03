using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Infrastructure.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        // Implement user-specific methods if needed
    }
}
