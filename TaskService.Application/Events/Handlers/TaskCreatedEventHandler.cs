using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Events;

namespace TaskService.Application.Events.Handlers
{
    public class TaskCreatedEventHandler : INotificationHandler<TaskCreatedEvent>

    {
        private readonly ILogger<TaskCreatedEventHandler> _logger;

        public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Task: '{TaskId}' '{TaskTitle}' created by user {UserId}",
                notification.TaskId,
                notification.Title,
                notification.UserId);


            // 1. Gửi notification
            // 2. Update cache
            // 3. Tính statistics
            // ...
            // execute somrething 
             await SimulateSendEmailAsync(notification, cancellationToken);

            // Ví dụ: Ghi log vào audit system
            await Task.Delay(100); // Simulate async work
            _logger.LogInformation("Task created event processed");
        }

        private async Task SimulateSendEmailAsync(TaskCreatedEvent notification, CancellationToken ct)
        {
            // Simulate async work
            await Task.Delay(100, ct);

            _logger.LogInformation(
                "[EMAIL] Email sent for new task: TaskId={TaskId}, Title={Title}",
                notification.TaskId,
                notification.Title);
        }
    }
}
