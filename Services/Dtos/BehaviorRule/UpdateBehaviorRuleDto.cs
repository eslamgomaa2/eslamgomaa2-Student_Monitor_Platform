using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRule
{
    public class UpdateBehaviorRuleDto
    {
        public int RuleID { get; set; }
        public string? RuleName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int SeverityLevel { get; set; }
    }
}
