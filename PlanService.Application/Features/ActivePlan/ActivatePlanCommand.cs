using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.ActivePlan
{
    public class ActivatePlanCommand : IRequest<Result>
    {
        public long PlanId { get; set; }
    }
}
