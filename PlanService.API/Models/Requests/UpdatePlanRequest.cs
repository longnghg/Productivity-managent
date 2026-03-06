using PlanService.Domain.Enums;

namespace PlanService.API.Models.Requests
{
    public class UpdatePlanRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PlanType Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PlanStatus Status { get; set; }
    }
}
