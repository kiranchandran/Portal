using Newtonsoft.Json;
using System.Net.Mime;
using System.Net;
using CompanyName.Model.Models;

namespace CompanyName.Api.IntegrationTests.ControllerIntegrationTests
{
    [Collection("IntegrationTestsCollection")]
    public class DepartmentControllerTests : IClassFixture<IntegrationTestWebApplicationFactory>
    {
        public DepartmentControllerTests(IntegrationTestWebApplicationFactory factory)
        {
            this.client = factory.Client;
        }

        private readonly HttpClient client;

        [Fact]
        public async Task GetDepartmentsTest()
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

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(result.Count == 4, "Returned incorrect no of departments.");
        }

        [Fact]
        public async Task GetDepartmentsByIdTest()
        {
            var allDepartments = await GetAllDepartments();

            var departmentId = allDepartments.First().Id;
            var httpMethod = HttpMethod.Get;
            var uri = $"api/Departments/{departmentId}";

            var request = new HttpRequestMessage(httpMethod, uri);
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);
            var result =
                JsonConvert
                    .DeserializeObject<DepartmentModel>(
                        httpResponse.Content.ReadAsStringAsync().Result);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(departmentId == result.Id);
        }

        [Fact]
        public async Task EnsureNotFoundWhenRequestingNonExistingDepartment()
        {
            var departmentId = Guid.NewGuid();
            var httpMethod = HttpMethod.Get;
            var uri = $"api/Departments/{departmentId}";

            var request = new HttpRequestMessage(httpMethod, uri);
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);
            var httpResponse = await this.client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        private async Task<List<DepartmentModel>> GetAllDepartments()
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
    }
}
