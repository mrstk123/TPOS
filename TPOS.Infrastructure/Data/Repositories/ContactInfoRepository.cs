// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System.Collections.Generic;
using System.Linq;	
using TPOS.Core.Interfaces.Repositories;
using TPOS.Core.Entities.Generated;

namespace TPOS.Infrastructure.Data.Repositories
{             
    public class ContactInfoRepository : Repository<ContactInfo>, IContactInfoRepository
    {
        public ContactInfoRepository(AppDbContext context) : base(context)
        {
        }

        //Override any generic method for your own custom implemention, and implement specific repository methods of the IContactInfoRepository.cs
    }
}
