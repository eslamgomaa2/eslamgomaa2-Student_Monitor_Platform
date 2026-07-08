using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public record BehaviorIncidentDto(
        int IncidentID,
    int StudentID,
    int RuleID,
    string? Source,
    string? Detail,
    decimal? Confidence,
    DateTime OccurredAt,
    int? ReviewedByUserID,
    ReviewStatus ReviewStatus
    );
}
