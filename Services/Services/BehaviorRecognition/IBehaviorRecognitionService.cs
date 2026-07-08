using Services.Dtos.BehaviorRecognation;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.BehaviorRecognition
{
    public interface IBehaviorRecognitionService
    {
        Task<BehaviorApiResponse> RecognizeBehaviorAsync(   Stream imageStream,    CancellationToken ct = default);
    }
}
