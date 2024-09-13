using Microsoft.Extensions.DependencyInjection;
using TPOS.Application.Interfaces;
using TPOS.Application.Interfaces.Services;
using TPOS.Infrastructure.Data;
using TPOS.Infrastructure.Security;

namespace TPOS.Infrastructure.Initialization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }

}
