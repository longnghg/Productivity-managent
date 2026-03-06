using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlanById
{
    internal class GetPlanByIdQueryHandler : IRequestHandler<GetPlanByIdQuery, Result<PlanDetailDto>>
    {
        private readonly IPlanRepository _planRepository;
        private readonly ILogger<GetPlanByIdQueryHandler> _logger;

        public GetPlanByIdQueryHandler(
            IPlanRepository planRepository,
            ILogger<GetPlanByIdQueryHandler> logger)
        {
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<Result<PlanDetailDto>> Handle(
            GetPlanByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting plan details for ID: {PlanId}", request.PlanId);

                var plan = await _planRepository.GetByIdWithDetailsAsync(
                    request.PlanId, cancellationToken);

                if (plan == null)
                {
                    _logger.LogWarning("Plan {PlanId} not found", request.PlanId);
                    return Result.Fail($"Plan with ID {request.PlanId} not found");
                }

                var result = new PlanDetailDto
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Type = plan.Type.ToString(),
                    StartDate = plan.StartDate,
                    EndDate = plan.EndDate,
                    Status = plan.Status.ToString(),
                    ProgressPercentage = plan.ProgressPercentage,
                    UserId = plan.UserId,
                    CreatedAt = plan.CreatedAt,
                    UpdatedAt = plan.UpdatedAt.GetValueOrDefault(),

                    Milestones = plan.Milestones.Select(m => new MilestoneDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        TargetDate = m.TargetDate,
                        Status = m.Status.ToString(),
                        CompletedAt = m.CompletedAt,
                        OrderIndex = m.OrderIndex,
                        IsOverdue = m.IsOverdue
                    }).OrderBy(m => m.OrderIndex).ToList(),

                    Tasks = plan.Tasks.Select(t => new PlanTaskDto
                    {
                        Id = t.Id,
                        MilestoneId = t.MilestoneId,
                        MilestoneName = plan.Milestones
                            .FirstOrDefault(m => m.Id == t.MilestoneId)?.Name ?? string.Empty,
                        ExternalTaskId = t.ExternalTaskId,
                        Title = t.Title,
                        Description = t.Description,
                        EstimatedMinutes = t.EstimatedMinutes,
                        Priority = t.Priority?.ToString() ?? "Medium",
                        Status = t.Status.ToString(),
                        CompletedAt = t.CompletedAt,
                        Tags = t.Tags,
                        OrderIndex = t.OrderIndex
                    }).OrderBy(t => t.OrderIndex).ToList()
                };

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting plan {PlanId}", request.PlanId);
                return Result.Fail("Error retrieving plan details");
            }
        }
    }
}
