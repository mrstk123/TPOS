using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPOS.Core.Entities.Generated;
using TPOS.Core.Interfaces.Repositories;

namespace TPOS.Infrastructure.Data.Repositories
{
    public class ContactInfoRepository : Repository<ContactInfo>, IContactInfoRepository
    {
        public ContactInfoRepository(AppDbContext context) : base(context)
        {
        }

        // Implement customer-specific methods if needed
    }
}
