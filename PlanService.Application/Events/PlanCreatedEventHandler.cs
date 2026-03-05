using MediatR;
using Microsoft.Extensions.Logging;
using PlanService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Application.Events
{
    public class PlanCreatedEventHandler : INotificationHandler<PlanCreatedEvent>
    {
        private readonly ILogger<PlanCreatedEventHandler> _logger;

        public PlanCreatedEventHandler(ILogger<PlanCreatedEventHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(PlanCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
               "[DOMAIN EVENT] PlanCreatedEvent: PlanId={PlanId}, PlanName={PlanName}, UserId={UserId}",
               notification.PlanId, notification.PlanName, notification.UserId);

            // TODO some logic business
            await Task.CompletedTask;
        }
    }
}
