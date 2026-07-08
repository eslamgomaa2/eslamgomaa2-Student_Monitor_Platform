using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.Dtos.FastApi
{
    public class FastApiLocation
    {

        [JsonPropertyName("top")]
        public int Top { get; set; }

        [JsonPropertyName("right")]
        public int Right { get; set; }

        [JsonPropertyName("bottom")]
        public int Bottom { get; set; }

        [JsonPropertyName("left")]
        public int Left { get; set; }
    }
}
