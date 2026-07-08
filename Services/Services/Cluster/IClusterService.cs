namespace Services.Services.Cluster
{
    using E_Learning.Core.Base;
    using global::Services.Dtos.Cluster;

    namespace SchoolSystem.Application.Services.Clustering.Interfaces
    {
        public interface IClusterService
        {
            Task<Response<ClusterRunResponseDto>> GetClusterSummariesAsync(ClusterFilterDto filters);
            Task<Response<GenerateClusterReportResponseDto>> GenerateClusterReportAsync(int TriggeredByUserID, GenerateClusterReportDto dto);
            Task<Response<StudentDetailDto>> GetStudentDetailsAsync(int studentId, int runId);
            Task<List<ClusterVisualizationPointDto>> GetClusterVisualizationDataAsync(int runId);
            Task<Response<string>> ResetFiltersAsync();
        }
    }
}
