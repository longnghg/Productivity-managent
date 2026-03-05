using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DomainEnum = TaskService.Domain.Enums;
using DomainEntity = TaskService.Domain.Entities;

namespace TaskService.Infras.Data.Configuration
{
    internal class TaskConfiguration : IEntityTypeConfiguration<DomainEntity.Task>
    {
        public void Configure(EntityTypeBuilder<DomainEntity.Task> builder)
        {
            builder.ToTable("tasks");

            // Primary Key với BIGINT IDENTITY
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()  // Database sẽ generate ID
                .UseIdentityColumn()     // PostgreSQL IDENTITY column
                .IsRequired();

            // Index cho phân trang mới nhất lên đầu
            builder.HasIndex(t => t.Id)
                .IsDescending()
                .HasDatabaseName("ix_tasks_id_desc")
                .HasFilter("is_deleted = false");

            // Column configurations
            builder.Property(t => t.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasColumnName("description")
                .HasMaxLength(2000);

            // Enum conversions
            builder.Property(t => t.Status)
                .HasColumnName("status")
                .HasConversion(
                    new ValueConverter<DomainEnum.TaskStatus, string>(
                        v => v.ToString(),
                        v => (DomainEnum.TaskStatus)Enum.Parse(typeof(DomainEnum.TaskStatus), v)))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.Priority)
                .HasColumnName("priority")
                .HasConversion(
                    new ValueConverter<DomainEnum.TaskPriority, string>(
                        v => v.ToString(),
                        v => (DomainEnum.TaskPriority)Enum.Parse(typeof(DomainEnum.TaskPriority), v)))
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.DueDate)
                .HasColumnName("due_date")
                .HasColumnType("timestamp with time zone");

            builder.Property(t => t.CompletedAt)
                .HasColumnName("completed_at")
                .HasColumnType("timestamp with time zone");

            builder.Property(t => t.EstimatedMinutes)
                .HasColumnName("estimated_minutes")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(t => t.ActualMinutes)
                .HasColumnName("actual_minutes")
                .HasColumnType("integer");

            builder.Property(t => t.UserId)
                .HasColumnName("user_id")
                .HasColumnType("bigint");

            builder.Property(t => t.ProjectId)
                .HasColumnName("project_id")
                .HasColumnType("bigint");

            // Timestamps
            builder.Property(t => t.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.StartedAt)
                .HasColumnName("started_at")
                .HasColumnType("timestamp with time zone");

            // Soft delete
            builder.Property(t => t.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false)
                .IsRequired();

            // Query filter soft delete
            builder.HasQueryFilter(t => !t.IsDeleted);

            // Table comment
            builder.ToTable(t => t.HasComment("Tasks table for productivity system"));
        }
    }
}
