using Microsoft.Extensions.DependencyInjection;
using TasksFilesApi.Services;
using TasksFilesApi.Services.Interfaces;

namespace TasksFilesApi.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IStorageService, FilesStorageService>();
        }
    }
}
