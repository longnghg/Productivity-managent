using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Events;

namespace TaskService.Application.Events.Handlers
{
    public class SendTaskCreatedEmailHandler : INotificationHandler<TaskCreatedEvent>
    {
        private readonly ILogger<TaskCreatedEventHandler> _logger;
        //private readonly IEmailService _emailService;

       
        public SendTaskCreatedEmailHandler(ILogger<TaskCreatedEventHandler> logger)
        {
            _logger = logger;

        }
        public async Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Demo send mail for task: {notification.Title} ");


            // 1. Gửi notification
            // 2. Update cache
            // 3. Tính statistics
            // ...

            await Task.Delay(100); 
            _logger.LogInformation("Sent mail event processed");


        }
    }
}
