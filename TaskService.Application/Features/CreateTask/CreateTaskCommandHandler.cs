using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.IRepositories;
using TaskService.Domain.Common;
using DomainEntity = TaskService.Domain.Entities;
namespace TaskService.Application.Features.CreateTask
{
    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<long>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTaskCommandHandler> _logger;
        private readonly IPublisher _publisher;
        public CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateTaskCommandHandler> logger,
        IPublisher publisher)
            {
                _taskRepository = taskRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
                _publisher = publisher;
            }
        public async Task<Result<long>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = DomainEntity.Task.Create(
                    request.Title,
                    request.Description,
                    request.Priority,
                    request.EstimatedMinutes,
                    request.UserId,
                    request.ProjectId,
                    dueDate: request.DueDate);

                // Add tag

                foreach (var tagName in request.Tags)
                {
                    task.AddTag(tagName);
                }

                await _taskRepository.AddAsync(task);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Task created with ID: {TaskId}", task.Id);

                // publish event
                await PublishDomainEvents(task,cancellationToken);
                return Result.Ok(task.Id);

            }
            catch (DomainException ex)
            {
                _logger.LogError($"DomainException, {ex.Message}");
                return Result.Fail($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception System Error, {ex.Message}");
                return Result.Fail($"System Error");

            }
        }

        private async Task PublishDomainEvents(DomainEntity.Task task, CancellationToken cancellationToken)
        {
            // Lấy tất cả domain events từ entity
            var domainEvents = task.DomainEvents;

            if (domainEvents == null || !domainEvents.Any())
                return;

            _logger.LogInformation(
                "Publishing {EventCount} domain events for TaskId={TaskId}",
                domainEvents.Count,
                task.Id);

            // Publish từng event
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
                _logger.LogDebug(
                    "Published event: {EventType}, EventId={EventId}",
                    domainEvent.GetType().Name,
                    domainEvent.EventId);
            }

            // Clear events sau khi publish
            task.ClearDomainEvents();
        }
    }
}
