using CompanyName.Data.Contexts;
using CompanyName.Data.Entity;
using Faker;

namespace CompanyName.Data.Seeds
{
    /// <summary>
    /// Responsible for seeing the initial data for testing purpose.
    /// </summary>
    public class DataInitialiser
    {
        private readonly DataContext context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">Data context <see cref="DataContext"/>.</param>
        public DataInitialiser(DataContext context)
        {
            this.context = context;
        }


        /// <summary>
        /// Seeds the departments and employee data.
        /// </summary>
        public void SeedInitialData()
        {
            #region Departments
            List<Department> departments = new()
            {
                new Department{ Name = "Administration" },
                new Department{ Name = "IT" },
                new Department{ Name = "Human Resources" },
                new Department{ Name = "Infrastructure" },
            };

            foreach (var item in departments)
            {
                bool isExisting = this.context.Departments.Any(s => s.Name.Equals(item.Name));
                if (!isExisting)
                {
                    this.context.Departments.Add(item);
                }
            }

            this.context.SaveChanges();

            #endregion

            #region Employees

            var random = new Random(100);
            for (int i = 0; i <= 20; i++)
            {
                Employee emp = new Employee
                {
                    Name = Faker.Name.FullName(NameFormats.WithPrefix)
                };
                emp.Email = Faker.Internet.Email(emp.Name);
                emp.DateOfBirth = DateTime.UtcNow.AddYears(-25).AddDays(random.Next(0, 1000));

                emp.Department = this.context.Departments.OrderBy(r => Guid.NewGuid()).First();
                this.context.Employees.Add(emp);
            }

            this.context.SaveChanges();
            #endregion
        }
    }
}
