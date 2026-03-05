using FluentResults;
using MediatR;
using PlanService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.CreatePlan
{
    public class CreatePlanCommand : IRequest<Result<long>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PlanType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserId { get; set; }

        public List<MilestoneRequest> Milestones { get; set; } = new();
        public List<PlanTaskRequest> Tasks { get; set; } = new();

        public class MilestoneRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime TargetDate { get; set; }
        }

        public class PlanTaskRequest
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int? EstimatedMinutes { get; set; }
            public TaskPriority? Priority { get; set; }
            public long? MilestoneId { get; set; }
            public List<string> Tags { get; set; } = new();
        }
    }
}
