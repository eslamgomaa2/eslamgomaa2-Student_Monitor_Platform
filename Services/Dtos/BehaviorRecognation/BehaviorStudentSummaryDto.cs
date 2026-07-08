using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.BehaviorRecognation
{
    public class BehaviorStudentSummaryDto
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int TotalIncidents { get; set; }
        public int PendingIncidents { get; set; }
        public int UnderReviewIncidents { get; set; }
        public int ConfirmedIncidents { get; set; }
        public int RejectedIncidents { get; set; }
        public DateTime? LastIncidentDate { get; set; }
    }
}
