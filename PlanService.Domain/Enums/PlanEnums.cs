using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Domain.Enums
{
    public enum PlanType
    {
        LearningSprint,  
        Project,        
        Goal,            
        Roadmap
    }

    public enum PlanStatus
    {
        Draft,      
        Active,     
        Completed,  
        Archived    
    }
}
