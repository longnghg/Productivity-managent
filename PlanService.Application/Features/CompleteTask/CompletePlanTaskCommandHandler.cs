using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Common;
using PlanService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.CompleteTask
{
    internal class CompletePlanTaskCommandHandler : IRequestHandler<CompletePlanTaskCommand, Result>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompletePlanTaskCommandHandler> _logger;

        public CompletePlanTaskCommandHandler(
            IPlanRepository planRepository,
            IUnitOfWork unitOfWork,
            ILogger<CompletePlanTaskCommandHandler> logger)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            CompletePlanTaskCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Completing task {PlanTaskId}", request.PlanTaskId);

                var planTask = await _planRepository.GetPlanTaskByIdAsync(
                    request.PlanTaskId, cancellationToken);

                if (planTask == null)
                {
                    return Result.Fail($"Task with ID {request.PlanTaskId} not found");
                }

                planTask.UpdateStatus(PlanTaskStatus.Completed, DateTime.UtcNow);
                await _planRepository.UpdatePlanTaskAsync(planTask, cancellationToken);

                // Update plan progress
                var plan = await _planRepository.GetByIdAsync(
                    planTask.PlanId, cancellationToken);

                if (plan != null)
                {
                    plan.UpdateProgress();
                    await _planRepository.UpdateAsync(plan, cancellationToken);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Task {PlanTaskId} completed, plan {PlanId} progress updated to {Progress}%",
                    request.PlanTaskId, planTask.PlanId, plan?.ProgressPercentage ?? 0);

                return Result.Ok();
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error completing task");
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing task");
                return Result.Fail("Error completing task");
            }
        }
    }
}
