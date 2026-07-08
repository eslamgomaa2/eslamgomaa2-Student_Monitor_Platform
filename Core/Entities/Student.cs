using System;
using System.Collections.Generic;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public string? FullName { get; set; }
        public string? NationalID { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? GradeLevel { get; set; }
        public string? Section { get; set; }
        public int AcademicYear { get; set; }
        public bool IsActive { get; set; } = true;
        
       
       

        // Navigation
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
        public ICollection<Grade> Grades { get; set; } = [];
        public ICollection<BehaviorIncident> BehaviorIncidents { get; set; } = [];
        public ICollection<StudentNote> StudentNotes { get; set; } = [];
        public ICollection<ClusterMember> ClusterMembers { get; set; } = [];
    }
}