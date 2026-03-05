using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Common
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
