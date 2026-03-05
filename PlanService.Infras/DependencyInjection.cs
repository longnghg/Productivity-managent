using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanService.Application.Interfaces;
using PlanService.Application.Interfaces.IRepositories;
using PlanService.Infras.Data;
using PlanService.Infras.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Infras
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<PlanDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("PlanDb"),
                    b => b.MigrationsAssembly(typeof(PlanDbContext).Assembly.FullName)));

            // Add Repositories
            services.AddScoped<IPlanRepository, PlanRepository>();

            // Add Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
