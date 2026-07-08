using Microsoft.Extensions.Configuration;
using Services.Dtos.BehaviorRecognation;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Services.BehaviorRecognition
{
    public class BehaviorRecognitionService : IBehaviorRecognitionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
       

        public BehaviorRecognitionService(
            IConfiguration config,
            HttpClient httpClient,
            IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _apiUrl = config["AiApiUrl"]
                ?? throw new InvalidOperationException("AiApiUrl not configured!");
          
        }

        public async Task<BehaviorApiResponse> RecognizeBehaviorAsync(  Stream imageStream, CancellationToken ct = default)
        {
            try
            {
                if (imageStream.CanSeek)
                    imageStream.Position = 0;

                // 1. إرسال الصورة للـ FastAPI
                using var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(imageStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent, "file", "camera.jpg");

                var response = await _httpClient.PostAsync($"{_apiUrl}/recognize-behavior", content, ct);
                var json = await response.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"📦 Behavior RAW JSON: '{json}'");

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException($"❌ AI Server error: {json}");

                // 2. Parse الـ Response
                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                };

                var apiResult = JsonSerializer.Deserialize<BehaviorApiResponse>(json, serializerOptions);

                
                return apiResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }
    }
}
