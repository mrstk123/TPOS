using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPOS.Core.Interfaces
{
    public interface IDbInitializer
    {
        Task SeedAsync();
    }
}
