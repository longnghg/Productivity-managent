
using FluentResults;
using MediatR;
using TaskService.Domain.Enums;

namespace TaskService.Application.Features.CreateTask
{
    public class CreateTaskCommand : IRequest<Result<long>>

    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public int EstimatedMinutes { get; set; }
        public DateTime? DueDate { get; set; }
        public long? ProjectId { get; set; }
        public long? UserId { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
