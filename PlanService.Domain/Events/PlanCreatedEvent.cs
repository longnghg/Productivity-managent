using PlanService.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Domain.Events
{
    public class PlanCreatedEvent : DomainEvent
    {
        public long PlanId { get; set; }
        public string PlanName { get; }
        public long? UserId { get; }

        public PlanCreatedEvent(long planId,
            string planName,
            long? userId)
        {
            PlanId = planId;
            PlanName = planName;
            UserId = userId;
        }
    }
}
