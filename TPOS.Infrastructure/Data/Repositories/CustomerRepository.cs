// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>

using System.Collections.Generic;
using System.Linq;	
using TPOS.Application.Interfaces.Repositories;
using TPOS.Domain.Entities.Generated;

namespace TPOS.Infrastructure.Data.Repositories
{             
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        //Override any generic method for your own custom implemention, and implement specific repository methods of the ICustomerRepository.cs
    }
}
