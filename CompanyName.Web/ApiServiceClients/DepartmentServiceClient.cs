using Blazored.Toast.Services;
using CompanyName.Model.Models;

namespace CompanyName.Web.ApiServiceClients
{
    public class DepartmentServiceClient : ServiceClientBase
    {
        public DepartmentServiceClient(HttpClient client, ILogger<DepartmentServiceClient> logger, IToastService toastService) : base(client, logger, toastService)
        {
        }

        public async Task<IList<DepartmentModel>> GetAsync(CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/Departments");
            var response = await this.ExecuteAsync<List<DepartmentModel>>(httpRequestMessage, cancellationToken);
            return response ?? new List<DepartmentModel>();
        }        

        public async Task<DepartmentModel?> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/Departments/{departmentId}");
            var response = await this.ExecuteAsync<DepartmentModel>(httpRequestMessage, cancellationToken);
            return response;
        }

    }
}
