using PlanService.Domain.Common;
using PlanService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Domain.Entities
{
    public class Milestone : EntityBase
    {
        // Properties
        public long PlanId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime TargetDate { get; private set; }
        public MilestoneStatus Status { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public int OrderIndex { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Constructor for EF Core
        private Milestone() { }

        // Internal constructor
        internal Milestone(
            long planId,
            string name,
            string description,
            DateTime targetDate,
            int orderIndex)
        {
            PlanId = planId;
            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            TargetDate = targetDate;
            Status = MilestoneStatus.Pending;
            OrderIndex = orderIndex;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Business methods
        public void Update(string name, string description, DateTime targetDate)
        {
            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;
            TargetDate = targetDate;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status == MilestoneStatus.Completed)
                throw new DomainException("Milestone is already completed");

            Status = MilestoneStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != MilestoneStatus.Pending)
                throw new DomainException("Only pending milestones can be started");

            Status = MilestoneStatus.InProgress;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsOverdue =>
            Status != MilestoneStatus.Completed &&
            TargetDate < DateTime.UtcNow;
    }
}
