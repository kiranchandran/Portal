using System.Text.Json.Serialization;

namespace CompanyName.Model.Models
{
    public class EmployeeSearchRequest
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("departmentId")]
        public Guid? DepartmentId { get; set; }
    }
}
