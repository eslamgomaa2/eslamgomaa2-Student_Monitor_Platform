using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Dtos.FastApi
{
    public class FastApiFaceResult
    {
        [JsonPropertyName("student_id")]
        public int StudentId { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("recognized")]
        public bool Recognized { get; set; }

        [JsonPropertyName("location")]
        public FastApiLocation? Location { get; set; }
    }
}
