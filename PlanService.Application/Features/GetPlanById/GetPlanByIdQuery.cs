using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlanById
{
    public class GetPlanByIdQuery : IRequest<Result<PlanDetailDto>>
    {
        public long PlanId { get; set; }
    }
    public class PlanDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ProgressPercentage { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<MilestoneDto> Milestones { get; set; } = new();
        public List<PlanTaskDto> Tasks { get; set; } = new();
    }

    public class MilestoneDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime TargetDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public int OrderIndex { get; set; }
        public bool IsOverdue { get; set; }
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
