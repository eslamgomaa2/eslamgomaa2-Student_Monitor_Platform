using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRule
{
    public class CreateBehaviorRuleDto
    {
        public string? RuleName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int SeverityLevel { get; set; }
        [Required]
        public int CreatedByUserID { get; set; }
    }
}
