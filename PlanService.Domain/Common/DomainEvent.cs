using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        public DateTime OccurredOn { get; set; }
        public Guid EventId { get; set; }
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}
