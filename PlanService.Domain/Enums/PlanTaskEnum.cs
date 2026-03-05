using System;
using System.Collections.Generic;
using System.Text;

namespace PlanService.Domain.Enums
{
    public enum PlanTaskStatus
    {
        Pending,    
        InProgress, 
        Completed,  
        Blocked,    
        Cancelled   
    }
}
