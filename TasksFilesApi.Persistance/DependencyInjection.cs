using TasksFilesApi.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksFilesApi.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace TasksFilesApi.Persistance
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainContext>(options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(MainContext).Assembly.FullName)));
            services.AddScoped<IMainContext>(provider => provider.GetService<MainContext>());
        }
    }
}
