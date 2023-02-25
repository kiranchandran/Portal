using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyName.Model.Models
{
    public class SaveEmployeeRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("departmentId")]
        public Guid DepartmentId { get; set; }
    }
}
