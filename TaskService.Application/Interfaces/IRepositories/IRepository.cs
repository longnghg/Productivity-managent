using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Application.Interfaces.IRepositories
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);
    }
}
