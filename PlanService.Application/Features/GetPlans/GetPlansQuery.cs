using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.GetPlans
{
    public class GetPlansQuery : IRequest<Result<List<PlanDto>>>
    {
        public long? UserId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class PlanDto
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

        // Calculated fields
        public int MilestoneCount { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public bool IsOverdue => EndDate < DateTime.UtcNow && Status != "Completed";
    }

}
