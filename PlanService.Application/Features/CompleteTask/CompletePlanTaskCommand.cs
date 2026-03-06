using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.CompleteTask
{
    public class CompletePlanTaskCommand : IRequest<Result>
    {
        public long PlanTaskId { get; set; }
    }
}
