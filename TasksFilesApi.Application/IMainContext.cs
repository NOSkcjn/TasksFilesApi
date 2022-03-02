using TasksFilesApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TasksFilesApi.Application
{
    public interface IMainContext
    {
        DbSet<ServiceFile> Files { get; set; }
        DbSet<ServiceTask> Tasks { get; set; }

        Task<int> SaveChangesAsync();
    }
}