// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System.Collections.Generic;
using System.Linq;	
using TPOS.Application.Interfaces.Repositories;
using TPOS.Domain.Entities.Generated;

namespace TPOS.Infrastructure.Data.Repositories
{             
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }

        //Override any generic method for your own custom implemention, and implement specific repository methods of the IPermissionRepository.cs
    }
}
