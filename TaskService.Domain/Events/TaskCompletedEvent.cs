using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Events
{
    public class TaskCompletedEvent : DomainEvent
    {
        public long TaskId { get; }
        public int ActualMinutes { get; }
        public int EstimatedMinutes { get; }

        public TaskCompletedEvent(long taskId, int actualMinutes, int estimatedMinutes)
        {
            TaskId = taskId;
            ActualMinutes = actualMinutes;
            EstimatedMinutes = estimatedMinutes;
        }
    }
}
