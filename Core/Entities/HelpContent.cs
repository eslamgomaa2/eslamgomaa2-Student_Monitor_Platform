namespace StudentBehaviorPlatform.Data.Entities
{
    public class HelpContent
    {
        public int ContentID { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? Tags { get; set; }
        public bool IsActive { get; set; } = true;
    }
}