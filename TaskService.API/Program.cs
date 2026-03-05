using Microsoft.EntityFrameworkCore;
using TaskService.API.Middleware;
using TaskService.Infras.Data;
using TaskService.Application; 
using TaskService.Infras; 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();





// 3. Add Application Layer (MediatR, AutoMapper, FluentValidation)
builder.Services.AddApplication();

// 4. Add Infrastructure Layer (DbContext, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Health Checks
builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



// Add custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();


// Map health checks
app.MapHealthChecks("/health");

// Database migration và seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TaskDbContext>();

        // Seed data
        await TaskDbContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}



app.Run();
