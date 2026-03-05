using PlanService.Domain.Common;
using PlanService.Domain.Enums;

namespace PlanService.Domain.Entities
{
    public class PlanTask : EntityBase
    {
        // Properties
        public long PlanId { get; private set; }
        public long? MilestoneId { get; private set; }

        // Reference to TaskService
        public long? ExternalTaskId { get; private set; }

        // Denormalized data
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int? EstimatedMinutes { get; private set; }
        public TaskPriority? Priority { get; private set; }
        public PlanTaskStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public List<string> Tags { get; private set; } = new();
        public long? AssignedTo { get; private set; }
        public int OrderIndex { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastSyncedAt { get; private set; }

        // Constructor for EF Core
        private PlanTask() { }

        // Internal constructor
        internal PlanTask(
            long planId,
            long? milestoneId,
            string title,
            string description,
            int? estimatedMinutes = null,
            TaskPriority? priority = null,
            int orderIndex = 1)
        {
            PlanId = planId;
            MilestoneId = milestoneId;
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            EstimatedMinutes = estimatedMinutes;
            Priority = priority;
            Status = PlanTaskStatus.Pending;
            OrderIndex = orderIndex;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Tags = new List<string>();
        }

        // Business methods
        public void UpdateFromTaskService(
            long externalTaskId,
            string title,
            string description,
            int? estimatedMinutes,
            TaskPriority? priority,
            List<string> tags)
        {
            ExternalTaskId = externalTaskId;
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            EstimatedMinutes = estimatedMinutes;
            Priority = priority;
            Tags = tags ?? new List<string>();
            LastSyncedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(PlanTaskStatus status, DateTime? completedAt = null)
        {
            Status = status;

            if (status == PlanTaskStatus.Completed && completedAt.HasValue)
            {
                CompletedAt = completedAt.Value;
            }
            else if (status != PlanTaskStatus.Completed)
            {
                CompletedAt = null;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignTo(long userId)
        {
            AssignedTo = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unassign()
        {
            AssignedTo = null;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTag(string tag)
        {
            if (!Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
            {
                Tags.Add(tag);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void RemoveTag(string tag)
        {
            if (Tags.RemoveAll(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase)) > 0)
            {
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void LinkToExternalTask(long externalTaskId)
        {
            ExternalTaskId = externalTaskId;
            LastSyncedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsCompleted => Status == PlanTaskStatus.Completed;
        public bool HasExternalTask => ExternalTaskId.HasValue;
    }
}