using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.CreatePlan
{
    public class CreatePlanCommandValidator : AbstractValidator<CreatePlanCommand>
    {
        public CreatePlanCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Plan name is required")
                .MaximumLength(200).WithMessage("Plan name cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid plan type");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be valid");

            // Validate milestones
            RuleForEach(x => x.Milestones)
                .ChildRules(milestone =>
                {
                    milestone.RuleFor(m => m.Name)
                        .NotEmpty().WithMessage("Milestone name is required")
                        .MaximumLength(200).WithMessage("Milestone name cannot exceed 200 characters");

                    milestone.RuleFor(m => m.Description)
                        .MaximumLength(500).WithMessage("Milestone description cannot exceed 500 characters");
                });

            // Validate tasks
            RuleForEach(x => x.Tasks)
                .ChildRules(task =>
                {
                    task.RuleFor(t => t.Title)
                        .NotEmpty().WithMessage("Task title is required")
                        .MaximumLength(200).WithMessage("Task title cannot exceed 200 characters");

                    task.RuleFor(t => t.Description)
                        .MaximumLength(1000).WithMessage("Task description cannot exceed 1000 characters");

                    task.RuleFor(t => t.EstimatedMinutes)
                        .GreaterThan(0).When(t => t.EstimatedMinutes.HasValue)
                        .WithMessage("Estimated minutes must be greater than 0");

                    task.RuleForEach(t => t.Tags)
                        .MaximumLength(50).WithMessage("Tag cannot exceed 50 characters");
                });
        }
    }
}
