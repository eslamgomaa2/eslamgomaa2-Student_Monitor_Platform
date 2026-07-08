using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Student
{
    public record AttendanceRecordDto(
        int AttendanceID,
        int StudentID,
        DateTime AttendanceDate,
        AttendanceStatus? Status,
        double? ConfidenceScore,
        string? Source
    );
}
