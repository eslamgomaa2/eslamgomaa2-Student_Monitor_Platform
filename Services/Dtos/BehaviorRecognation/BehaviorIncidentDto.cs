using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRecognation
{
    public class BehaviorIncidentDto
    {
        public int IncidentID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int RuleID { get; set; }
        public string? RuleName { get; set; }
        public string? Source { get; set; }
        public string? Detail { get; set; }
        public decimal? Confidence { get; set; }
        public DateTime OccurredAt { get; set; }
        public int? ReviewedByUserID { get; set; }
        public string? ReviewedByUserName { get; set; }
        public ReviewStatus ReviewStatus { get; set; }
        public string ReviewStatusDisplay => ReviewStatus.ToString();
    }
}
