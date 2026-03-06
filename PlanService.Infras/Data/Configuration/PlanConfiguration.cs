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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> entity)
        {
            entity.ToTable("plans");

            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasColumnName("description");

            entity.Property(p => p.Type)
                .HasColumnName("plan_type")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(p => p.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(p => p.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(PlanStatus.Draft);

            entity.Property(p => p.ProgressPercentage)
                .HasColumnName("progress_percentage")
                .HasDefaultValue(0);

            entity.Property(p => p.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            entity.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            // Navigation properties
            entity.HasMany(p => p.Milestones)
                .WithOne()
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Tasks)
                .WithOne()
                .HasForeignKey(pt => pt.PlanId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
