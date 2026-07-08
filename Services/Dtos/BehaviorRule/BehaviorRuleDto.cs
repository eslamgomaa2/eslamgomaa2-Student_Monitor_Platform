using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRule
{
    public class BehaviorRuleDto
    {
        public int RuleID { get; set; }
        public string? RuleName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int SeverityLevel { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
