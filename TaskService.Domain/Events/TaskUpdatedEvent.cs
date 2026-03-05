using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Events
{
    public class TaskUpdatedEvent : DomainEvent
    {
        public long TaskId { get; set; }

        public TaskUpdatedEvent(long taskId)
        {
            TaskId = taskId;
        }
    }
}
