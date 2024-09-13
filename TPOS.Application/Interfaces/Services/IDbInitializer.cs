using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOS.Application.Interfaces.Services
{
    public interface IDbInitializer
    {
        Task SeedAsync();
    }
}
