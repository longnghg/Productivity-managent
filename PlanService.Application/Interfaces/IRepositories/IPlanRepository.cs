using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PlanService.Application.Interfaces.IRepositories
{
    public interface IPlanRepository : IRepository<Plan>
    {
        // Special methods for Plan
        Task<Plan?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default);
        Task<List<Plan>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);

        // Milestone methods
        Task<Milestone?> GetMilestoneByIdAsync(long milestoneId, CancellationToken cancellationToken = default);
        Task<List<Milestone>> GetMilestonesByPlanIdAsync(long planId, CancellationToken cancellationToken = default);

        // PlanTask methods
        Task<PlanTask?> GetPlanTaskByIdAsync(long planTaskId, CancellationToken cancellationToken = default);
        Task<PlanTask?> GetPlanTaskByExternalIdAsync(long externalTaskId, CancellationToken cancellationToken = default);
        Task<List<PlanTask>> GetPlanTasksByPlanIdAsync(long planId, CancellationToken cancellationToken = default);
        Task UpdatePlanTaskAsync(PlanTask planTask, CancellationToken cancellationToken = default);
    }
}
