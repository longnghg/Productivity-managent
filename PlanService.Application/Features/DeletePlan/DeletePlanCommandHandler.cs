using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.DeletePlan
{
    internal class DeletePlanCommandHandler : IRequestHandler<DeletePlanCommand, Result>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePlanCommandHandler> _logger;

        public DeletePlanCommandHandler(
            IPlanRepository planRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeletePlanCommandHandler> logger)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DeletePlanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting plan {PlanId}", request.PlanId);

                var plan = await _planRepository.GetByIdAsync(
                    request.PlanId, cancellationToken);

                if (plan == null)
                {
                    _logger.LogWarning("Plan {PlanId} not found", request.PlanId);
                    return Result.Fail($"Plan with ID {request.PlanId} not found");
                }

                await _planRepository.DeleteAsync(plan, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Plan {PlanId} deleted successfully", request.PlanId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting plan {PlanId}", request.PlanId);
                return Result.Fail("Error deleting plan");
            }
        }
    }
}
