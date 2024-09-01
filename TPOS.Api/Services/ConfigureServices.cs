using TPOS.Infrastructure.DbInitializer;

namespace TPOS.Api.Services
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();

            // services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }
    }
}
