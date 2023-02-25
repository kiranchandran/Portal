using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;
using CompanyName.Model.Models;
using System.Text;

namespace CompanyName.Api.IntegrationTests.ControllerIntegrationTests
{
    [Collection("IntegrationTestsCollection")]
    public class EmployeeControllerTests : IClassFixture<IntegrationTestWebApplicationFactory>
    {
        public EmployeeControllerTests(IntegrationTestWebApplicationFactory factory)
        {
            this.client = factory.Client;
        }

        private readonly HttpClient client;

        [Fact]
        public async Task GetEmployees()
        {
            var httpMethod = HttpMethod.Post;
            var uri = "api/Employees/Search";

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var json = JsonConvert.SerializeObject(new EmployeeSearchRequest
            {

            });
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);
            var result =
                JsonConvert
                    .DeserializeObject<List<EmployeeModel>>(
                        httpResponse.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(result.Count > 20, "Returned incorrect no of employees.");
        }

        [Fact]
        public async Task EnsureBadRequestRetunedIfNameEmpty()
        {
            var httpMethod = HttpMethod.Post;
            var uri = "api/Employees";

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var departments = await GetDepartments();

            var json = JsonConvert.SerializeObject(new SaveEmployeeRequest
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DepartmentId = departments.First().Id,
                Email = "Test@test.com",
                Name = string.Empty
            });
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);

            var result =
                JsonConvert
                    .DeserializeObject<BadRequestErrorModel>(
                        httpResponse.Content.ReadAsStringAsync().Result);

            Assert.True(result.Errors.Count == 1);
        }

        [Fact]
        public async Task CreateNewEmployee()
        {
            var httpMethod = HttpMethod.Post;
            var uri = "api/Employees";

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var departments = await GetDepartments();
            var newEmployee = new SaveEmployeeRequest
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DepartmentId = departments.First().Id,
                Email = "Test@test.com",
                Name = "New Employee"
            };
            var json = JsonConvert.SerializeObject(newEmployee);
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);

            var result =
                JsonConvert
                    .DeserializeObject<EmployeeModel>(
                        httpResponse.Content.ReadAsStringAsync().Result);

            Assert.Equal(newEmployee.Name, result.Name);
            Assert.Equal(newEmployee.Email, result.Email);
            Assert.Equal(newEmployee.DepartmentId, result.DepartmentId);
        }

        [Fact]
        public async Task EnsureEmployeeIsDeleted()
        {
            //Step 1 : Create a new employee
            var httpMethod = HttpMethod.Post;
            var uri = "api/Employees";

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var departments = await GetDepartments();
            var newEmployee = new SaveEmployeeRequest
            {
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                DepartmentId = departments.First().Id,
                Email = "Test@test.com",
                Name = "New Employee"
            };
            var json = JsonConvert.SerializeObject(newEmployee);
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);

            var result =
                JsonConvert
                    .DeserializeObject<EmployeeModel>(
                        httpResponse.Content.ReadAsStringAsync().Result);

            var newEmployeeId = result.Id;

            //Step 2 : Get the total employee count

            var totalEmployeeCountBeforeDelete = (await GetAllEmployees()).Count();

            //Step 3 : Delete the newly created employee

            httpMethod = HttpMethod.Delete;
            uri = $"api/Employees/{newEmployeeId}";
            request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);
            httpResponse = await this.client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            //Step 4 : Check the employee count again.

            var totalEmployeeCountAfterDelete = (await GetAllEmployees()).Count();

            Assert.True(totalEmployeeCountAfterDelete == (totalEmployeeCountBeforeDelete - 1));
        }

        private async Task<List<DepartmentModel>> GetDepartments()
        {
            var httpMethod = HttpMethod.Get;
            var uri = "api/Departments";

            var request = new HttpRequestMessage(httpMethod, uri);
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);
            var result =
                JsonConvert
                    .DeserializeObject<List<DepartmentModel>>(
                        httpResponse.Content.ReadAsStringAsync().Result);
            return result;
        }

        private async Task<List<EmployeeModel>> GetAllEmployees()
        {
            var httpMethod = HttpMethod.Post;
            var uri = "api/Employees/Search";

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var json = JsonConvert.SerializeObject(new EmployeeSearchRequest
            {

            });
            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);
            var result =
                JsonConvert
                    .DeserializeObject<List<EmployeeModel>>(
                        httpResponse.Content.ReadAsStringAsync().Result);
            return result;
        }
    }
}
