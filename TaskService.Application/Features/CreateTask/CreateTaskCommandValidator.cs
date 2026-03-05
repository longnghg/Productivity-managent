using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Features.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.EstimatedMinutes)
                .GreaterThan(0).WithMessage("Estimated minutes must be greater than 0")
                .LessThanOrEqualTo(1440).WithMessage("Estimated minutes cannot exceed 24 hours (1440 minutes)");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date cannot be in the past");

            RuleForEach(x => x.Tags)
                .MaximumLength(50).WithMessage("Tag cannot exceed 50 characters");
        }
    }
}
