using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Dtos.FastApi
{
    public class FastApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("faces_found")]
        public int FacesFound { get; set; }

        [JsonPropertyName("results")]
        public List<FastApiFaceResult> Results { get; set; } = new();
    }
}
