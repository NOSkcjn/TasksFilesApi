using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TasksFilesApi.Application;
using TasksFilesApi.Domain.Entities;

namespace TasksFilesApi.Persistance.Context
{
    public class MainContext : DbContext, IMainContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceTask>()
                .HasMany(x => x.Files)
                .WithOne(x => x.Task).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ServiceFile> Files { get; set; }

        public DbSet<ServiceTask> Tasks { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
