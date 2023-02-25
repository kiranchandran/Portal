using Blazored.Toast.Services;
using CompanyName.Model.Models;
using System.Net.Mime;
using System.Text.Json;

namespace CompanyName.Web.ApiServiceClients
{
    public abstract class ServiceClientBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientBase" /> class.
        /// </summary>
        /// <param name="client">The instance of <see cref="System.Net.Http.HttpClient" />.</param>
        /// <param name="logger">Instance of <see cref="ILogger" />.</param>
        /// <param name="toastService">Toaster service <see cref="IToastService"/>.</param>
        public ServiceClientBase(HttpClient client, ILogger<ServiceClientBase> logger, IToastService toastService)
        {
            this.HttpClient = client;
            this.Logger = logger;
            this.ToastService = toastService;
        }

        protected IToastService ToastService { get; }

        protected HttpClient HttpClient { get; }

        protected ILogger<ServiceClientBase> Logger { get; }

        /// <inheritdoc />
        protected virtual async Task ExecuteAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            try
            {
                this.ToastService?.ClearErrorToasts();
                if (!request.Headers.Accept.Any(x =>
                    x.MediaType != null && x.MediaType.Equals(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)))
                {
                    request.Headers.Add("Accept", MediaTypeNames.Application.Json);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var response = await this.HttpClient.SendAsync(request, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    HandleError(response, content);
                }
                
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                this.Logger.LogError(ex, "Timed out: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                this.Logger.LogError(ex, "Request cancelled: {Message}", ex.Message);
                throw;
            }
            catch (WebApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Request failed: {Message}", ex.Message);
                ToastService.ShowError(ex.Message);
            }
        }

        /// <inheritdoc />
        protected virtual async Task<T?> ExecuteAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default)
            where T : new()
        {
            T? result = default;
            try
            {
                this.ToastService?.ClearErrorToasts();
                if (!request.Headers.Accept.Any(x =>
                    x.MediaType != null && x.MediaType.Equals(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)))
                {
                    request.Headers.Add("Accept", MediaTypeNames.Application.Json);
                }

                cancellationToken.ThrowIfCancellationRequested();

                var response = await this.HttpClient.SendAsync(request, cancellationToken);
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonSerializer.Deserialize<T>(content);
                }
                else
                {
                    HandleError(response, content);
                }

                return result;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                this.Logger.LogError(ex, "Timed out: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                this.Logger.LogError(ex, "Request cancelled: {Message}", ex.Message);
                throw;
            }
            catch (WebApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Request failed: {Message}", ex.Message);
                throw;
            }
        }

        private void HandleError(HttpResponseMessage response, string content)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                this.Logger.LogError("External API Error", content); ;
                var errorResponse = JsonSerializer.Deserialize<BadRequestErrorModel>(content);
                if (errorResponse != null && errorResponse.Errors != null)
                {
                    ToastService.ClearAll();
                    foreach (var item in errorResponse.Errors)
                    {
                        foreach (var it in item.Value.ToList())
                        {
                            if (!string.IsNullOrWhiteSpace(it))
                            {
                                ToastService.ShowError(it);
                            }
                        }
                    }
                }
            }
            else
            {
                this.Logger.LogError("External API Error", response); ;
                var errorMessage = $"Status code :{response.StatusCode} and error message :{response.ReasonPhrase}";
                this.Logger.LogError(errorMessage);

                throw new WebApiException(errorMessage);
            }
        }
    }
}
