using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TaskService.Application.Interfaces;
using TaskService.Application.Interfaces.IRepositories;
using TaskService.Infras.Data;
using TaskService.Infras.Data.Repositories;

namespace TaskService.Infras
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config
            )
        {

            // db

            services.AddDbContext<TaskDbContext>(options =>
                options.UseNpgsql(
                      config.GetConnectionString("TaskDb"),
                      x => x.MigrationsAssembly(typeof(TaskDbContext).Assembly.FullName)));

            // repo

            services.AddScoped<ITaskRepository, TaskRepository>();

            // uow
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;

        }
    }
}
