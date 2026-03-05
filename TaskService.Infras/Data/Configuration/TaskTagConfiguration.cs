using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Domain.Entities;

namespace TaskService.Infras.Data.Configuration
{
    public class TaskTagConfiguration : IEntityTypeConfiguration<TaskTag>
    {
        public void Configure(EntityTypeBuilder<TaskTag> builder)
        {
            builder.ToTable("task_tags");

            builder.HasKey(tt => tt.Id);

            builder.Property(tt => tt.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .UseIdentityColumn()
                .IsRequired();

            builder.Property(tt => tt.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(tt => tt.TaskId)
                .HasColumnName("task_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(tt => tt.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            // Index cho unique tag name per task
            builder.HasIndex(tt => new { tt.TaskId, tt.Name })
                .IsUnique()
                .HasDatabaseName("ix_task_tags_task_id_name_unique");

            // Index cho tìm kiếm tag
            builder.HasIndex(tt => tt.Name)
                .HasDatabaseName("ix_task_tags_name");

        }
    }
}
