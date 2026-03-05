using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;

namespace TaskService.Domain.Entities
{
    public class TaskTag : EntityBase
    {
        public string Name { get; private set; }
        public long TaskId { get; private set; }
        public Task Task { get; private set; }


        public static TaskTag Create(string name, long taskId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Tag name cannot be empty");

            return new TaskTag
            {
                Name = name.Trim().ToLowerInvariant(),
                TaskId = taskId
            };
        }
    }
}
