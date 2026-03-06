using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.DeletePlan
{
    public class DeletePlanCommand : IRequest<Result>
    {
        public long PlanId { get; set; }
    }
}
