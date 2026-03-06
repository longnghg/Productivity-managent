using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.UpdatePlan
{
    public class UpdatePlanCommandValidator : AbstractValidator<UpdatePlanCommand>
    {
        public UpdatePlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Plan name is required")
                .MaximumLength(200).WithMessage("Plan name cannot exceed 200 characters");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before or equal to end date");
        }
    }
}
