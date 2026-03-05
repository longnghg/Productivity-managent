using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Events
{
    public class TaskStartedEvent : DomainEvent
    {
        public long TaskId { get; }

        public TaskStartedEvent(long taskId)
        {
            TaskId = taskId;
        }
    }
}
