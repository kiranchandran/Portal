using System.Text.Json.Serialization;

namespace CompanyName.Model.Models
{
    public class DepartmentModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
