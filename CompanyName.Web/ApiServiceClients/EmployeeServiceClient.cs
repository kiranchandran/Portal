using Blazored.Toast.Services;
using CompanyName.Model.Models;

namespace CompanyName.Web.ApiServiceClients
{
    public class EmployeeServiceClient : ServiceClientBase
    {
        public EmployeeServiceClient(HttpClient client, ILogger<EmployeeServiceClient> logger,  IToastService toastService) : base(client, logger, toastService)
        {
        }

        public async Task<IList<EmployeeModel>> SearchAsync(EmployeeSearchRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/Employees/Search").WithJsonContent(request);
            var response = await this.ExecuteAsync<List<EmployeeModel>>(httpRequestMessage, cancellationToken);
            return response ?? new List<EmployeeModel>();
        }

        public Task<EmployeeModel?> CreateAsync(SaveEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/Employees").WithJsonContent(request);
            return this.ExecuteAsync<EmployeeModel>(httpRequestMessage, cancellationToken);
        }

        public Task<EmployeeModel?> UpdateAsync(Guid employeeId, SaveEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, $"/api/Employees/{employeeId}").WithJsonContent(request);
            return this.ExecuteAsync<EmployeeModel>(httpRequestMessage, cancellationToken);
        }

        public Task<EmployeeModel?> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/Employees/{employeeId}");
            return this.ExecuteAsync<EmployeeModel>(httpRequestMessage, cancellationToken);
        }

        public Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/api/Employees/{employeeId}");
            return this.ExecuteAsync(httpRequestMessage, cancellationToken);
        }
    }
}
