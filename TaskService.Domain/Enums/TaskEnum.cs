using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Domain.Enums
{
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Archived = 4,
        Cancelled = 5
    }

    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }

}
