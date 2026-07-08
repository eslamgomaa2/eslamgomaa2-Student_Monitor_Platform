namespace StudentMonitor.Core.Models;

public class FaceRecognitionResult
{
    public int StudentId { get; set; }
    public double Confidence { get; set; }
    public bool IsRecognized => StudentId > 0; 
}