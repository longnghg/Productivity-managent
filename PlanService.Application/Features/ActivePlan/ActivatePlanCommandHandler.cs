using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.ActivePlan
{
    internal class ActivatePlanCommandHandler : IRequestHandler<ActivatePlanCommand, Result>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActivatePlanCommandHandler> _logger;

        public ActivatePlanCommandHandler(
            IPlanRepository planRepository,
            IUnitOfWork unitOfWork,
            ILogger<ActivatePlanCommandHandler> logger)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ActivatePlanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Activating plan {PlanId}", request.PlanId);

                var plan = await _planRepository.GetByIdAsync(
                    request.PlanId, cancellationToken);

                if (plan == null)
                {
                    return Result.Fail($"Plan with ID {request.PlanId} not found");
                }

                plan.Activate();
                await _planRepository.UpdateAsync(plan, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Plan {PlanId} activated", request.PlanId);
                return Result.Ok();
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error activating plan");
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating plan");
                return Result.Fail("Error activating plan");
            }
        }
    }
}
