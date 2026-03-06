using FluentResults;
using MediatR;
using PlanService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.UpdatePlan
{
    public class UpdatePlanCommand : IRequest<Result>
    {
        public long PlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PlanType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PlanStatus Status { get; set; }
    }
}
