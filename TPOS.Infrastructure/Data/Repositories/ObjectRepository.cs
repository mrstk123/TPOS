// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System.Collections.Generic;
using System.Linq;	
using TPOS.Application.Interfaces.Repositories;
using TPOS.Domain.Entities.Generated;
using Object = TPOS.Domain.Entities.Generated.Object;

namespace TPOS.Infrastructure.Data.Repositories
{             
    public class ObjectRepository : Repository<Object>, IObjectRepository
    {
        public ObjectRepository(AppDbContext context) : base(context)
        {
        }

        //Override any generic method for your own custom implemention, and implement specific repository methods of the IObjectRepository.cs
    }
}
