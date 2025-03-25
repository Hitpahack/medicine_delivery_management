using Microsoft.Extensions.DependencyInjection;

namespace RepMed.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection InjectSSMervices(this IServiceCollection services, string connectionString = "")
        {
            services.AddMemoryCache();
            //services.AddDbContext<careuappContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMasterService, MasterService>();
            return services;
        }
    }
}
