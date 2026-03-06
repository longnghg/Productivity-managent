using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.UpdatePlan
{
    internal class UpdatePlanCommandHandler : IRequestHandler<UpdatePlanCommand, Result>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePlanCommandHandler> _logger;

        public UpdatePlanCommandHandler(
            IPlanRepository planRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdatePlanCommandHandler> logger)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            UpdatePlanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating plan {PlanId}", request.PlanId);

                var plan = await _planRepository.GetByIdAsync(
                    request.PlanId, cancellationToken);

                if (plan == null)
                {
                    _logger.LogWarning("Plan {PlanId} not found", request.PlanId);
                    return Result.Fail($"Plan with ID {request.PlanId} not found");
                }

                plan.Update(
                    request.Name,
                    request.Description,
                    request.Type,
                    request.StartDate,
                    request.EndDate);

                await _planRepository.UpdateAsync(plan, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Plan {PlanId} updated successfully", request.PlanId);
                return Result.Ok();
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error updating plan {PlanId}", request.PlanId);
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating plan {PlanId}", request.PlanId);
                return Result.Fail("Error updating plan");
            }
        }
    }
}
