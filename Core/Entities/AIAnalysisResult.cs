using System;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class AIAnalysisResult
    {
        public int ResultID { get; set; }
        public int SessionID { get; set; }
        public string? AnalysisType { get; set; }
        public string? ResultPayload { get; set; }
        public decimal? OverallConfidence { get; set; }
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public VideoSession? VideoSession { get; set; }
    }
}