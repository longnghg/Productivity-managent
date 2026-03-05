using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Common;
using TaskService.Domain.Enums;
using TaskService.Domain.Events;
using EnumEntity = TaskService.Domain.Enums;

namespace TaskService.Domain.Entities
{
    public  class Task : EntityBase, IAggregateRoot
    {
        private string _title;
        private string _description;
        private EnumEntity.TaskStatus _status;
        private EnumEntity.TaskPriority _priority;
        private DateTime? _dueDate;
        private DateTime? _completedAt;
        private int _estimatedMinutes;
        private int? _actualMinutes;
        private readonly List<TaskTag> _tags = new();
        private DateTime? _startedAt;

        // Properties với controlled access
        public string Title
        {
            get => _title;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomainException("Task title cannot be empty");

                if (value.Length > 200)
                    throw new DomainException("Task title cannot exceed 200 characters");

                _title = value;
                UpdateTimestamps();
            }
        }

        public string Description
        {
            get => _description;
            private set
            {
                _description = value;
                UpdateTimestamps();
            }
        }

        public EnumEntity.TaskStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                UpdateTimestamps();
            }
        }

        public EnumEntity.TaskPriority Priority
        {
            get => _priority;
            private set
            {
                _priority = value;
                UpdateTimestamps();
            }
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            private set
            {
                _dueDate = value;
                UpdateTimestamps();
            }
        }

        public DateTime? CompletedAt
        {
            get => _completedAt;
            private set => _completedAt = value;
        }
        public DateTime? StartedAt
        {
            get => _startedAt;
            private set => _startedAt = value;
        }
        public int EstimatedMinutes
        {
            get => _estimatedMinutes;
            private set
            {
                if (value < 0)
                    throw new DomainException("Estimated minutes cannot be negative");

                _estimatedMinutes = value;
                UpdateTimestamps();
            }
        }

        public int? ActualMinutes
        {
            get => _actualMinutes;
            private set
            {
                if (value < 0)
                    throw new DomainException("Actual minutes cannot be negative");

                _actualMinutes = value;
                UpdateTimestamps();
            }
        }

        public long? UserId { get; private set; }
        public long? ProjectId { get; private set; }
        // Navigation properties (readonly)
        public IReadOnlyCollection<TaskTag> Tags => _tags.AsReadOnly();


        public static Task Create(
       string title,
       string description,
       TaskPriority priority,
       int estimatedMinutes,
       long? userId = null,
       long? projectId = null,
       DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required");

            if (estimatedMinutes <= 0)
                throw new DomainException("Estimated minutes must be greater than 0");

            var task = new Task
            {
                Title = title,
                Description = description,
                Priority = priority,
                Status = EnumEntity.TaskStatus.Pending,
                EstimatedMinutes = estimatedMinutes,
                UserId = userId,
                ProjectId = projectId,
                DueDate = dueDate.HasValue ? dueDate.Value.ToUniversalTime() : null
            };

            // Raise domain event
            task.AddDomainEvent(new TaskCreatedEvent(task.Id, task.Title, task.UserId));

            return task;
        }

        // Domain methods (business logic)
        public void UpdateDetails(string title, string description, EnumEntity.TaskPriority priority)
        {
            Title = title;
            Description = description;
            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new TaskUpdatedEvent(Id));
        }
        public void StartProgress()
        {
            if (Status == EnumEntity.TaskStatus.Completed)
                throw new DomainException("Cannot start a completed task");

            Status = EnumEntity.TaskStatus.InProgress;
            StartedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new TaskStartedEvent(Id));
        }
        public void Complete()
        {
            if (Status == EnumEntity.TaskStatus.Completed)
                throw new DomainException("Task is already completed");
            if (Status == EnumEntity.TaskStatus.Pending)
                throw new DomainException("Task is pending");

            Status = EnumEntity.TaskStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            int actualMinutes = (int)(CompletedAt.Value - StartedAt!.Value).TotalMinutes;
            ActualMinutes = actualMinutes;

            AddDomainEvent(new TaskCompletedEvent(Id, actualMinutes, EstimatedMinutes));
        }
        public void AddTag(string tagName)
        {
            if (_tags.Any(t => t.Name == tagName))
                throw new DomainException($"Tag '{tagName}' already exists");

            _tags.Add(TaskTag.Create(tagName, Id));
            UpdateTimestamps();
        }

        public void RemoveTag(string tagName)
        {
            var tag = _tags.FirstOrDefault(t => t.Name == tagName);
            if (tag != null)
            {
                _tags.Remove(tag);
                UpdateTimestamps();
            }

        }

        public void Reschedule(DateTime newDueDate)
        {
            if (newDueDate < DateTime.UtcNow)
                throw new DomainException("Due date cannot be in the past");

            DueDate = newDueDate;
            AddDomainEvent(new TaskRescheduledEvent(Id, newDueDate));

        }

        public bool IsOverdue() => DueDate.HasValue && DueDate < DateTime.UtcNow;

        public bool IsHighPriority() => Priority >= TaskPriority.High;
        public int? GetTimeVariance() => ActualMinutes.HasValue
        ? ActualMinutes.Value - EstimatedMinutes
        : null;
    }
}
