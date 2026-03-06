using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanService.Domain.Entities;
using PlanService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace PlanService.Infras.Data.Configuration
{
    public class PlanTaskConfiguration : IEntityTypeConfiguration<PlanTask>
    {
        public void Configure(EntityTypeBuilder<PlanTask> entity)
        {

            entity.ToTable("plan_tasks");

            entity.HasKey(pt => pt.Id);
            entity.Property(pt => pt.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(pt => pt.PlanId)
                .HasColumnName("plan_id")
                .IsRequired();

            entity.Property(pt => pt.MilestoneId)
                .HasColumnName("milestone_id");

            // Reference to TaskService (denormalized)
            entity.Property(pt => pt.ExternalTaskId)
                .HasColumnName("external_task_id");

            // Denormalized task data
            entity.Property(pt => pt.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(pt => pt.Description)
                .HasColumnName("description");

            entity.Property(pt => pt.EstimatedMinutes)
                .HasColumnName("estimated_minutes");

            entity.Property(pt => pt.Priority)
                .HasColumnName("priority")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(pt => pt.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(PlanTaskStatus.Pending);

            entity.Property(pt => pt.CompletedAt)
                .HasColumnName("completed_at")
                .HasColumnType("timestamptz");

            entity.Property(pt => pt.Tags)
                .HasColumnName("tags")
                .HasColumnType("text[]")
                .HasDefaultValueSql("'{}'");

            entity.Property(pt => pt.AssignedTo)
                .HasColumnName("assigned_to");

            entity.Property(pt => pt.OrderIndex)
                .HasColumnName("order_index");

            entity.Property(pt => pt.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            entity.Property(pt => pt.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            entity.Property(pt => pt.LastSyncedAt)
                .HasColumnName("last_synced_at")
                .HasColumnType("timestamptz");

            // Relationships
            entity.HasOne<Plan>()
                .WithMany(p => p.Tasks)
                .HasForeignKey(pt => pt.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Milestone>()
                .WithMany()
                .HasForeignKey(pt => pt.MilestoneId)
                .OnDelete(DeleteBehavior.SetNull);



        }
    }
}
