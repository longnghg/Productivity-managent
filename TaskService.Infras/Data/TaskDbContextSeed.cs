using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DomainEnum = TaskService.Domain.Enums;
using DomainEntity = TaskService.Domain.Entities;

namespace TaskService.Infras.Data
{
    public static class TaskDbContextSeed
    {
        public static async Task SeedAsync(TaskDbContext context)
        {
            // Chỉ seed nếu database chưa có data
            if (!await context.Tasks.AnyAsync())
            {
                var tasks = new List<DomainEntity.Task>
                {
                    DomainEntity.Task.Create(
                        title: "Setup development environment",
                        description: "Install .NET 8, Docker, PostgreSQL",
                        priority: DomainEnum.TaskPriority.High,
                        estimatedMinutes: 120,
                        userId: 1,
                        dueDate: DateTime.UtcNow.AddDays(1)),

                    DomainEntity.Task.Create(
                        title: "Learn Clean Architecture",
                        description: "Study domain-driven design and layer separation",
                        priority: DomainEnum.TaskPriority.Medium,
                        estimatedMinutes: 180,
                        userId: 1,
                        dueDate: DateTime.UtcNow.AddDays(3)),

                    DomainEntity.Task.Create(
                        title: "Implement TaskService",
                        description: "Create CRUD operations for tasks",
                        priority: DomainEnum.TaskPriority.High,
                        estimatedMinutes: 240,
                        userId: 1,
                        dueDate: DateTime.UtcNow.AddDays(2))
                };

                // Add tags
                tasks[0].AddTag("setup");
                tasks[0].AddTag("development");
                tasks[1].AddTag("learning");
                tasks[1].AddTag("architecture");
                tasks[2].AddTag("implementation");
                tasks[2].AddTag("backend");

                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
