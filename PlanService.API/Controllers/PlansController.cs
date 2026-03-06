using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlanService.API.Models.Requests;
using PlanService.Application.Features.ActivePlan;
using PlanService.Application.Features.CompleteTask;
using PlanService.Application.Features.CreatePlan;
using PlanService.Application.Features.DeletePlan;
using PlanService.Application.Features.GetPlanById;
using PlanService.Application.Features.GetPlans;
using PlanService.Application.Features.GetPlanTasks;
using PlanService.Application.Features.UpdatePlan;

namespace PlanService.API.Controllers
{
    [ApiController]
    [Route("api/v1/plans")]
    public class PlansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/plans
        [HttpGet]
        public async Task<IActionResult> GetPlans(
            [FromQuery] long? userId,
            [FromQuery] string? status,
            [FromQuery] string? type,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetPlansQuery
            {
                UserId = userId,
                Status = status,
                Type = type,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new
            {
                Plans = result.Value,
                Page = page,
                PageSize = pageSize,
                TotalCount = result.Value.Count
            });
        }

        // GET: api/v1/plans/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(long id)
        {
            var query = new GetPlanByIdQuery { PlanId = id };
            var result = await _mediator.Send(query);

            if (result.IsFailed)
                return NotFound(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(result.Value);
        }

        // GET: api/v1/plans/{id}/tasks
        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetPlanTasks(
            long id,
            [FromQuery] string? status,
            [FromQuery] long? milestoneId)
        {
            var query = new GetPlanTasksQuery
            {
                PlanId = id,
                Status = status,
                MilestoneId = milestoneId
            };

            var result = await _mediator.Send(query);

            if (result.IsFailed)
                return NotFound(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(result.Value);
        }

        // POST: api/v1/plans
        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanRequest request)
        {
            var command = new CreatePlanCommand
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                UserId = request.UserId,
                Milestones = request.Milestones?.Select(m => new CreatePlanCommand.MilestoneRequest
                {
                    Name = m.Name,
                    Description = m.Description,
                    TargetDate = m.TargetDate
                }).ToList() ?? new(),
                Tasks = request.Tasks?.Select(t => new CreatePlanCommand.PlanTaskRequest
                {
                    Title = t.Title,
                    Description = t.Description,
                    EstimatedMinutes = t.EstimatedMinutes,
                    Priority = t.Priority,
                    MilestoneId = t.MilestoneId,
                    Tags = t.Tags
                }).ToList() ?? new()
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new { PlanId = result.Value });
        }

        // PUT: api/v1/plans/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(long id, [FromBody] UpdatePlanRequest request)
        {
            var command = new UpdatePlanCommand
            {
                PlanId = id,
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new { Message = "Plan updated successfully" });
        }

        // DELETE: api/v1/plans/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(long id)
        {
            var command = new DeletePlanCommand { PlanId = id };
            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return NotFound(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new { Message = "Plan deleted successfully" });
        }

        // POST: api/v1/plans/{id}/activate
        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivatePlan(long id)
        {
            var command = new ActivatePlanCommand { PlanId = id };
            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new { Message = "Plan activated successfully" });
        }

        // PUT: api/v1/plans/tasks/{taskId}/complete
        [HttpPut("tasks/{taskId}/complete")]
        public async Task<IActionResult> CompleteTask(long taskId)
        {
            var command = new CompletePlanTaskCommand { PlanTaskId = taskId };
            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            return Ok(new { Message = "Task completed successfully" });
        }

        // GET: api/v1/plans/stats
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] long? userId)
        {
            // Simple stats for now
            var query = new GetPlansQuery
            {
                UserId = userId,
                PageSize = 1000 // Get many plans
            };

            var result = await _mediator.Send(query);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors.Select(e => e.Message) });

            var stats = new
            {
                TotalPlans = result.Value.Count,
                ActivePlans = result.Value.Count(p => p.Status == "Active"),
                CompletedPlans = result.Value.Count(p => p.Status == "Completed"),
                DraftPlans = result.Value.Count(p => p.Status == "Draft"),
                OverduePlans = result.Value.Count(p => p.IsOverdue),
                TotalTasks = result.Value.Sum(p => p.TaskCount),
                CompletedTasks = result.Value.Sum(p => p.CompletedTaskCount)
            };

            return Ok(stats);
        }
    }
}