using PlanService.Domain.Common;
using PlanService.Domain.Enums;
using PlanService.Domain.Events;
using System.Numerics;

namespace PlanService.Domain.Entities
{
    public class Plan : EntityBase, IAggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public PlanType Type { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public PlanStatus Status { get; private set; }
        public int ProgressPercentage { get; private set; }
        public long UserId { get; private set; }

        // Navigation properties
        private readonly List<Milestone> _milestones = new();
        public IReadOnlyCollection<Milestone> Milestones => _milestones.AsReadOnly();

        private readonly List<PlanTask> _tasks = new();
        public IReadOnlyCollection<PlanTask> Tasks => _tasks.AsReadOnly();

        public static Plan Create(
           string name,
           string description,
           PlanType type,
           DateTime startDate,
           DateTime endDate,
           long userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Plan name is required");

            if (endDate < startDate)
                throw new DomainException("End date must be after start date");

            var plan = new Plan
            {
                Name = name,
                Description = description,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
                Status = PlanStatus.Draft,
                ProgressPercentage = 0,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            plan.AddDomainEvent(new PlanCreatedEvent(plan.Id, plan.Name, plan.UserId));
            return plan;
        }

        public void Update(
          string name,
          string description,
          PlanType type,
          DateTime startDate,
          DateTime endDate,
          long userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Plan name is required");

            if (endDate < startDate)
                throw new DomainException("End date must be after start date");

            this.Name = name;
            this.Description = description;
            this.Type = type;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Status = PlanStatus.Draft;
            this.ProgressPercentage = 0;
            this.UserId = userId;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;

            this.AddDomainEvent(new PlanCreatedEvent(this.Id, this.Name, this.UserId));
        }

        public void UpdateProgress()
        {
            // Tính progress từ milestones và tasks
            var totalItems = _milestones.Count + _tasks.Count;
            if (totalItems == 0)
            {
                ProgressPercentage = 0;
                return;
            }

            var completedItems =
                _milestones.Count(m => m.Status == MilestoneStatus.Completed) +
                _tasks.Count(t => t.Status == PlanTaskStatus.Completed);

            ProgressPercentage = (int)((completedItems / (double)totalItems) * 100);

            // Update status
            if (ProgressPercentage == 100)
            {
                Status = PlanStatus.Completed;
            }
            else if (ProgressPercentage > 0)
            {
                Status = PlanStatus.Active;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status != PlanStatus.Draft)
                throw new DomainException("Only draft plans can be activated");

            Status = PlanStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }


        public void AddMilestone(string name, string description, DateTime targetDate)
        {
            var milestone = new Milestone(
                planId: Id,
                name: name,
                description: description,
                targetDate: targetDate,
                orderIndex: _milestones.Count + 1
            );

            _milestones.Add(milestone);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
