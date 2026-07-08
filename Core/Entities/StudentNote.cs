using System;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class StudentNote
    {
        public int NoteID { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }
        public string? NoteText { get; set; }
        public string? NoteType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Student? Student { get; set; }
        public ApplicationUser? User { get; set; }
    }
}