using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlans
{
    internal class GetPlansQueryHandler : IRequestHandler<GetPlansQuery, Result<List<PlanDto>>>
    {
        private readonly IPlanRepository _planRepository;
        private readonly ILogger<GetPlansQueryHandler> _logger;

        public GetPlansQueryHandler(
            IPlanRepository planRepository,
            ILogger<GetPlansQueryHandler> logger)
        {
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<Result<List<PlanDto>>> Handle(
            GetPlansQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Getting plans for user {UserId}, page {Page}, size {PageSize}",
                    request.UserId, request.Page, request.PageSize);

                var query = _planRepository.GetQueryable()
                    .Include(p => p.Milestones)
                    .Include(p => p.Tasks)
                    .AsQueryable();

                // Apply filters
                if (request.UserId.HasValue)
                {
                    query = query.Where(p => p.UserId == request.UserId);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(p => p.Status.ToString() == request.Status);
                }

                if (!string.IsNullOrEmpty(request.Type))
                {
                    query = query.Where(p => p.Type.ToString() == request.Type);
                }

                // Pagination
                var skip = (request.Page - 1) * request.PageSize;
                var plans = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip(skip)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var result = plans.Select(p => new PlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Type = p.Type.ToString(),
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status.ToString(),
                    ProgressPercentage = p.ProgressPercentage,
                    UserId = p.UserId,
                    CreatedAt = p.CreatedAt,
                    MilestoneCount = p.Milestones.Count,
                    TaskCount = p.Tasks.Count,
                    CompletedTaskCount = p.Tasks.Count(t => t.Status.ToString() == "Completed")
                }).ToList();

                _logger.LogInformation("Found {Count} plans", result.Count);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plans");
                return Result.Fail("Error retrieving plans");
            }
        }
    }
}
