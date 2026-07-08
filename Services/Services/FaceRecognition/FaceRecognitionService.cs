using Microsoft.Extensions.Configuration;
using Services.Dtos.FaceRecognition;
using Services.Dtos.FastApi;
using Services.Services.FaceRecognition;
using StudentMonitor.Core.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StudentBehaviorPlatform.Services.Services;

public class FaceRecognitionService : IFaceRecognitionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public FaceRecognitionService(
        IConfiguration config,
        HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiUrl = config["AiApiUrl"]
            ?? throw new InvalidOperationException("AiApiUrl not configured!");
    }

    public async Task<FaceRecognitionResult> RecognizeFaceAsync(Stream imageStream, CancellationToken ct = default)
    {
        try
        {
            // إعادة ضبط Stream لو استُخدم قبل كذا
            if (imageStream.CanSeek)
                imageStream.Position = 0;

            // 1. تجهيز multipart/form-data
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(imageStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "file", "camera.jpg");

            // 2. إرسال الطلب
            var response = await _httpClient.PostAsync($"{_apiUrl}/recognize", content, ct);

            // 3. قراءة الـ JSON
            var json = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"📦 RAW JSON: '{json}'");
            Console.WriteLine($"📦 Length: {json?.Length ?? 0}");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"❌ AI Server error: {json}");
            }

            // 4. تسلسل الـ JSON (يدعم Object و Array)
            List<FastApiFaceResult> results = null;

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            // محاولة 1: { "results": [...] }
            var apiResult = JsonSerializer.Deserialize<FastApiResponse>(json, serializerOptions);
            if (apiResult?.Results != null && apiResult.Results.Count > 0)
            {
                results = apiResult.Results;
            }
            else
            {
                // محاولة 2: مصفوفة مباشرة [...]
                results = JsonSerializer.Deserialize<List<FastApiFaceResult>>(json, serializerOptions);
            }

            if (results == null || results.Count == 0)
            {
                Console.WriteLine("⚠️ لا توجد نتائج في الـ Response");
                return new FaceRecognitionResult { StudentId = 0, Confidence = 0 };
            }

            // 5. اختيار أعلى Confidence
            var bestMatch = results
                .Where(r => r.Recognized)
                .OrderByDescending(r => r.Confidence)
                .FirstOrDefault();

            if (bestMatch == null)
            {
                return new FaceRecognitionResult { StudentId = 0, Confidence = 0 };
            }

            var result = new FaceRecognitionResult
            {
                StudentId = bestMatch.StudentId,
                Confidence = bestMatch.Confidence,
            };

            Console.WriteLine($"🔍 StudentId={result.StudentId}, Confidence={result.Confidence}, IsRecognized={result.IsRecognized}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Exception: {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> RegisterFaceAsync(int studentId, string studentCode, string studentName, Stream imageStream, CancellationToken ct = default)
    {
        try
        {
            if (imageStream.CanSeek)
                imageStream.Position = 0;

            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(studentId.ToString()), "student_id");
            content.Add(new StringContent(studentCode), "student_code");
            content.Add(new StringContent(studentName), "student_name");

            var fileContent = new StreamContent(imageStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "file", $"{studentCode}.jpg");

            var response = await _httpClient.PostAsync($"{_apiUrl}/register-face", content, ct);

            var json = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"📦 Register RAW JSON: '{json}'");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"❌ AI Server error: {response.StatusCode} - {json}");
            }

            var result = JsonSerializer.Deserialize<RegisterApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            return result?.Success ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 Register Exception: {ex.GetType().Name}: {ex.Message}");
            throw;
        }
    }
}