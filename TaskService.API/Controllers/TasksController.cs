using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskService.API.Models.Requests;
using TaskService.Application.Features.CreateTask;
using TaskService.Domain.Enums;

namespace TaskService.API.Controllers
{
    [Route("api/v1/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                Priority = (TaskPriority)request.Priority,
                EstimatedMinutes = request.EstimatedMinutes,
                UserId = request.UserId,
                ProjectId = request.ProjectId,
                DueDate = request.DueDate,
                Tags = request.Tags
            };
            
            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(result.Errors);

            return Ok(new { TaskId = result.Value });
        }
    }
}
