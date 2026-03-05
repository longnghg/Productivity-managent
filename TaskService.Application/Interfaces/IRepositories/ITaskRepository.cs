using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Common.Models;
using TaskService.Domain.Enums;
using TaskStatus = TaskService.Domain.Enums.TaskStatus;
using DomainEntity = TaskService.Domain.Entities;

namespace TaskService.Application.Interfaces.IRepositories
{
    public interface ITaskRepository : IRepository<DomainEntity.Task>
    {
        Task<List<DomainEntity.Task>> GetTasksByUserAsync(long userId, CancellationToken cancellationToken = default);
        Task<List<DomainEntity.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken = default);
        Task<List<DomainEntity.Task>> GetTasksByProjectAsync(long projectId, CancellationToken cancellationToken = default);

        // Pagination methods
        Task<PaginatedList<DomainEntity.Task>> GetPaginatedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<DomainEntity.Task>> GetPaginatedWithFilterAsync(
            TaskStatus? status,
            TaskPriority? priority,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
