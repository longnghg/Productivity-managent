using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Events
{
    public class TaskCreatedEvent : DomainEvent
    {
        public long TaskId { get; set; }
        public string Title { get; }
        public long? UserId { get; }

        public TaskCreatedEvent(long taskId, string title, long? userId)
        {
            TaskId = taskId;
            Title = title;
            UserId = userId;
        }


    }
}
