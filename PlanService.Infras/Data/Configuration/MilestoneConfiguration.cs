using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace PlanService.Infras.Data.Configuration
{
    internal class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
    {
        public void Configure(EntityTypeBuilder<Milestone> entity)
        {
            entity.ToTable("milestones");

            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(m => m.PlanId)
                .HasColumnName("plan_id")
                .IsRequired();

            entity.Property(m => m.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(m => m.Description)
                .HasColumnName("description");

            entity.Property(m => m.TargetDate)
                .HasColumnName("target_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(m => m.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.Property(m => m.CompletedAt)
                .HasColumnName("completed_at")
                .HasColumnType("timestamptz");

            entity.Property(m => m.OrderIndex)
                .HasColumnName("order_index")
                .IsRequired();

            entity.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            entity.Property(m => m.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            // Relationships
            entity.HasOne<Plan>()
                .WithMany(p => p.Milestones)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            //// Indexes
            //entity.HasIndex(m => m.PlanId)
            //    .HasDatabaseName("idx_milestones_plan_id");

            //entity.HasIndex(m => m.Status)
            //    .HasDatabaseName("idx_milestones_status");

            //entity.HasIndex(m => m.TargetDate)
            //    .HasDatabaseName("idx_milestones_target_date");



        }
    }
}
