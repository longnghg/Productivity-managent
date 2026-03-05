using Microsoft.EntityFrameworkCore;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Entities;
using PlanService.Infras.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PlanService.Infras.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        private readonly PlanDbContext _context;
        private readonly DbSet<Plan> _dbSet;

        public PlanRepository(PlanDbContext context) : base(context) 
        {
            _context = context;
            _dbSet = context.Set<Plan>();
        }

        // Basic CRUD operations
        public async Task<Plan?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<Plan>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Plan>> GetAsync(
            Expression<Func<Plan, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<Plan> AddAsync(Plan entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task UpdateAsync(Plan entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Plan entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(Expression<Func<Plan, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<Plan, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(predicate, cancellationToken);
        }

        // Special methods for Plan
        public async Task<Plan?> GetByIdWithDetailsAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.Milestones)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Plan>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(p => p.Id == id, cancellationToken);
        }

        // Milestone methods
        public async Task<Milestone?> GetMilestoneByIdAsync(long milestoneId, CancellationToken cancellationToken = default)
        {
            return await _context.Milestones
                .FirstOrDefaultAsync(m => m.Id == milestoneId, cancellationToken);
        }

        public async Task<List<Milestone>> GetMilestonesByPlanIdAsync(long planId, CancellationToken cancellationToken = default)
        {
            return await _context.Milestones
                .Where(m => m.PlanId == planId)
                .OrderBy(m => m.OrderIndex)
                .ToListAsync(cancellationToken);
        }

        // PlanTask methods
        public async Task<PlanTask?> GetPlanTaskByIdAsync(long planTaskId, CancellationToken cancellationToken = default)
        {
            return await _context.PlanTasks
                .FirstOrDefaultAsync(pt => pt.Id == planTaskId, cancellationToken);
        }

        public async Task<PlanTask?> GetPlanTaskByExternalIdAsync(long externalTaskId, CancellationToken cancellationToken = default)
        {
            return await _context.PlanTasks
                .FirstOrDefaultAsync(pt => pt.ExternalTaskId == externalTaskId, cancellationToken);
        }

        public async Task<List<PlanTask>> GetPlanTasksByPlanIdAsync(long planId, CancellationToken cancellationToken = default)
        {
            return await _context.PlanTasks
                .Where(pt => pt.PlanId == planId)
                .OrderBy(pt => pt.OrderIndex)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdatePlanTaskAsync(PlanTask planTask, CancellationToken cancellationToken = default)
        {
            _context.PlanTasks.Update(planTask);
            await Task.CompletedTask;
        }
    }
}
