using Microsoft.EntityFrameworkCore;
using PlanService.Domain.Common;
using PlanService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Infras.Data
{
    public class PlanDbContext : DbContext
    {
        public PlanDbContext(DbContextOptions options) : base(options)
        {
        }


        // DbSets
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<PlanTask> PlanTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();
            base.OnModelCreating(modelBuilder);

            // Apply all configurations từ assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlanDbContext).Assembly);
        }
    }
}
