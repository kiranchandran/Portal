using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyName.Model.Models
{
    public class EmployeeModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }
       
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("dateOfBirth")]
        [Display(Name = "Date Of Birth", Description = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonPropertyName("departmentName")]
        public string DepartmentName { get; set; }
    }
}
