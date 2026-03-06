using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlanTasks
{
    internal class GetPlanTasksQueryHandler : IRequestHandler<GetPlanTasksQuery, Result<List<PlanTaskDto>>>
    {
        private readonly IPlanRepository _planRepository;
        private readonly ILogger<GetPlanTasksQueryHandler> _logger;

        public GetPlanTasksQueryHandler(
            IPlanRepository planRepository,
            ILogger<GetPlanTasksQueryHandler> logger)
        {
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<Result<List<PlanTaskDto>>> Handle(
            GetPlanTasksQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Getting tasks for plan {PlanId} with status {Status}",
                    request.PlanId, request.Status);

                var plan = await _planRepository.GetByIdWithDetailsAsync(
                    request.PlanId, cancellationToken);

                if (plan == null)
                {
                    return Result.Fail($"Plan with ID {request.PlanId} not found");
                }

                var tasks = plan.Tasks.AsQueryable();

                if (!string.IsNullOrEmpty(request.Status))
                {
                    tasks = tasks.Where(t => t.Status.ToString() == request.Status);
                }

                if (request.MilestoneId.HasValue)
                {
                    tasks = tasks.Where(t => t.MilestoneId == request.MilestoneId);
                }

                var result = tasks.Select(t => new PlanTaskDto
                {
                    Id = t.Id,
                    MilestoneId = t.MilestoneId,
                    MilestoneName = plan.Milestones
                        .First(m => m.Id == t.MilestoneId).Name,
                    ExternalTaskId = t.ExternalTaskId,
                    Title = t.Title,
                    Description = t.Description,
                    EstimatedMinutes = t.EstimatedMinutes,
                    Priority = t.Priority.ToString(),
                    Status = t.Status.ToString(),
                    CompletedAt = t.CompletedAt,
                    Tags = t.Tags,
                    OrderIndex = t.OrderIndex
                }).OrderBy(t => t.OrderIndex).ToList();

                _logger.LogInformation("Found {Count} tasks", result.Count);
                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tasks for plan {PlanId}", request.PlanId);
                return Result.Fail("Error retrieving plan tasks");
            }
        }
    }
}
