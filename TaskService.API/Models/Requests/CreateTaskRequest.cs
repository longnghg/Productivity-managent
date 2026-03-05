namespace TaskService.API.Models.Requests
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int EstimatedMinutes { get; set; }
        public long? UserId { get; set; }
        public long? ProjectId { get; set; }
        public DateTime? DueDate { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
