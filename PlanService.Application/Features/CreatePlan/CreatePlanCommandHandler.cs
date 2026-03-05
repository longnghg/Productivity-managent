using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Domain.Common;
using PlanService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Features.CreatePlan
{
    internal class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, Result<long>>
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;
        private readonly ILogger<CreatePlanCommandHandler> _logger;

        public CreatePlanCommandHandler(
            IPlanRepository planRepository,
            IUnitOfWork unitOfWork,
            IPublisher publisher,
            ILogger<CreatePlanCommandHandler> logger)
        {
            _planRepository = planRepository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(
            CreatePlanCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Creating plan: {PlanName}, UserId: {UserId}",
                    request.Name, request.UserId);

                // 1. Create Plan aggregate
                var plan = Domain.Entities.Plan.Create(
                    request.Name,
                    request.Description,
                    request.Type,
                    request.StartDate,
                    request.EndDate,
                    request.UserId);

                // 2. Add milestones
                foreach (var milestoneRequest in request.Milestones)
                {
                    plan.AddMilestone(
                        milestoneRequest.Name,
                        milestoneRequest.Description,
                        milestoneRequest.TargetDate);
                }

                //// 3. Add tasks
                //foreach (var taskRequest in request.Tasks)
                //{
                //    plan.AddTask(
                //        taskRequest.Title,
                //        taskRequest.Description,
                //        taskRequest.EstimatedMinutes,
                //        taskRequest.Priority,
                //        taskRequest.MilestoneId);
                //}

                // 4. Save to database
                await _planRepository.AddAsync(plan, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Plan created with ID: {PlanId}, Milestones: {MilestoneCount}, Tasks: {TaskCount}",
                    plan.Id, plan.Milestones.Count, plan.Tasks.Count);

                // 5. Publish domain events
                await PublishDomainEvents(plan, cancellationToken);

                return Result.Ok(plan.Id);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error creating plan");
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "System error creating plan");
                return Result.Fail("System error. Please try again.");
            }
        }

        private async Task PublishDomainEvents(
            Plan plan,
            CancellationToken cancellationToken)
        {
            var domainEvents = plan.DomainEvents.ToList();

            if (!domainEvents.Any())
                return;

            _logger.LogDebug(
                "Publishing {EventCount} domain events for Plan {PlanId}",
                domainEvents.Count, plan.Id);

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            plan.ClearDomainEvents();
        }
    }
}
