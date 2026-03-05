using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlanService.API.Models.Requests;
using PlanService.Application.Features.CreatePlan;

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
                Milestones = request.Milestones.Select(m => new CreatePlanCommand.MilestoneRequest
                {
                    Name = m.Name,
                    Description = m.Description,
                    TargetDate = m.TargetDate
                }).ToList(),
                Tasks = request.Tasks.Select(t => new CreatePlanCommand.PlanTaskRequest
                {
                    Title = t.Title,
                    Description = t.Description,
                    EstimatedMinutes = t.EstimatedMinutes,
                    Priority = t.Priority,
                    MilestoneId = t.MilestoneId,
                    Tags = t.Tags
                }).ToList()
            };

            var result = await _mediator.Send(command);

            if (result.IsFailed)
                return BadRequest(new { Errors = result.Errors });

            return Ok(new { PlanId = result.Value });
        }
    }
}
