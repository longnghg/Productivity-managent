using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Events
{
    public class TaskRescheduledEvent : DomainEvent
    {
        public long TaskId { get; set; }
        public DateTime NewDueDate { get; set; }

        public TaskRescheduledEvent(long taskId, DateTime newDuedate)
        {
            
            TaskId = taskId;
            NewDueDate = newDuedate;
        }
    }
}
