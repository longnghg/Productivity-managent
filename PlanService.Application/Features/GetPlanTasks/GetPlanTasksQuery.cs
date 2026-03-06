using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlanTasks
{
    public class GetPlanTasksQuery : IRequest<Result<List<PlanTaskDto>>>
    {
        public long PlanId { get; set; }
        public string? Status { get; set; }
        public long? MilestoneId { get; set; }
    }

    public class PlanTaskDto
    {
        public long Id { get; set; }
        public long? MilestoneId { get; set; }
        public string MilestoneName { get; set; } = string.Empty;
        public long? ExternalTaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? EstimatedMinutes { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public List<string> Tags { get; set; } = new();
        public int OrderIndex { get; set; }
        public bool IsLinked => ExternalTaskId.HasValue;
    }
}
