using System.Text.Json.Serialization;

namespace CompanyName.Model.Models
{
    public class BadRequestErrorModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
