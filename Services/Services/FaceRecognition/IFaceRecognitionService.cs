using StudentMonitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.FaceRecognition
{
 

    public interface IFaceRecognitionService
    {
        Task<FaceRecognitionResult> RecognizeFaceAsync(  Stream imageStream,  CancellationToken ct = default);
        Task<bool> RegisterFaceAsync(int studentId, string? studentCode, string studentName, Stream faceImageStream, CancellationToken ct = default);
    }
}
