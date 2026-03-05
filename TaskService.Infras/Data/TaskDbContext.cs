using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Common;
using DomainEntity = TaskService.Domain.Entities;

namespace TaskService.Infras.Data
{
    public class TaskDbContext : DbContext
    {
        public DbSet<DomainEntity.Task> Tasks => Set<DomainEntity.Task>();
        public DbSet<DomainEntity.TaskTag> TaskTags => Set<DomainEntity.TaskTag>();

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bỏ qua DomainEvent trong model
            modelBuilder.Ignore<DomainEvent>();
            base.OnModelCreating(modelBuilder);

            // Apply all configurations từ assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {

            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
