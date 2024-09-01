using Microsoft.Extensions.DependencyInjection;
using TPOS.Core.Interfaces;
using TPOS.Infrastructure.Services;

namespace TPOS.Infrastructure.Initialization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IAuthService, AuthService>();
            // services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }

}
