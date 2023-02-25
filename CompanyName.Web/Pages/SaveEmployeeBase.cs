using Blazored.Toast.Services;
using CompanyName.Model.Models;
using CompanyName.Web.ApiServiceClients;
using Microsoft.AspNetCore.Components;

namespace CompanyName.Web.Pages
{
    public class SaveEmployeeBase : ComponentBase
    {
        [Inject]
        public EmployeeServiceClient EmployeeServiceClient { get; set; }

        public EmployeeModel Employee { get; set; } = new EmployeeModel();

        [Inject]
        public DepartmentServiceClient DepartmentServiceClient { get; set; }

        public List<DepartmentModel> Departments { get; set; } = new List<DepartmentModel>();

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }


        public bool IsEditMode
        {
            get
            {
                return !string.IsNullOrEmpty(Id);
            }
        }
        protected async override Task OnInitializedAsync()
        {
            Departments = (await DepartmentServiceClient.GetAsync()).ToList();

            if (IsEditMode)
            {
                var employee = await EmployeeServiceClient.GetByIdAsync(Guid.Parse(Id));

                if (employee != null)
                {
                    Employee = employee;
                }
                else
                {
                    ToastService.ClearAll();
                    ToastService.ShowError("Failed to fetch employee");
                }
            }
            else
            {
                Employee = new EmployeeModel();
                Employee.DepartmentId = Departments?.FirstOrDefault()?.Id ?? Guid.Empty;
            }
           
        }

        protected async Task HandleValidSubmit()
        {
            if (IsEditMode)
            {
                var result = await EmployeeServiceClient.UpdateAsync(Employee.Id, new SaveEmployeeRequest
                {
                    DateOfBirth = Employee.DateOfBirth,
                    DepartmentId = Employee.DepartmentId,
                    Email = Employee.Email,
                    Name = Employee.Name
                });
                if (result != null)
                {
                    ToastService.ShowSuccess("Employee details updated successfully.");
                    NavigationManager.NavigateTo("/employees");
                }
            }
            else
            {
                var result = await EmployeeServiceClient.CreateAsync(new SaveEmployeeRequest
                {
                    DateOfBirth = Employee.DateOfBirth,
                    DepartmentId = Employee.DepartmentId,
                    Email = Employee.Email,
                    Name = Employee.Name
                });
                if (result != null)
                {
                    ToastService.ShowSuccess("New Employee created successfully.");
                    NavigationManager.NavigateTo("/employees");
                }
            }
           
        }
    }
}
