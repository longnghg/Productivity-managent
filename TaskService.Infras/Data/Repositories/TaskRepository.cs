using Microsoft.EntityFrameworkCore;
using TaskService.Application.Common.Models;
using TaskService.Application.Interfaces.IRepositories;
using DomainEnum = TaskService.Domain.Enums;
using DomainEntity = TaskService.Domain.Entities;

namespace TaskService.Infras.Data.Repositories
{
    public class TaskRepository : BaseRepository<DomainEntity.Task>, ITaskRepository
    {
        public TaskRepository(TaskDbContext context) : base(context)
        {
        }

        public async Task<List<DomainEntity.Task>> GetTasksByUserAsync(
            long userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Id) // Mới nhất lên đầu
                .Include(t => t.Tags)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DomainEntity.Task>> GetOverdueTasksAsync(
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .Where(t => t.DueDate < now &&
                           t.Status != DomainEnum.TaskStatus.Completed &&
                           !t.IsDeleted)
                .OrderByDescending(t => t.Id)
                .Include(t => t.Tags)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<DomainEntity.Task>> GetTasksByProjectAsync(
            long projectId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Id)
                .Include(t => t.Tags)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<DomainEntity.Task>> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .OrderByDescending(t => t.Id)
                .Include(t => t.Tags);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<DomainEntity.Task>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PaginatedList<DomainEntity.Task>> GetPaginatedWithFilterAsync(
            DomainEnum.TaskStatus? status,
            DomainEnum.TaskPriority? priority,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            query = query
                .OrderByDescending(t => t.Id)
                .Include(t => t.Tags);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<DomainEntity.Task>(items, totalCount, pageNumber, pageSize);
        }

        // Helper method để lấy IQueryable (cho complex queries)
        public IQueryable<DomainEntity.Task> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
