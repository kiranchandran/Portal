namespace CompanyName.Data.Entity
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
