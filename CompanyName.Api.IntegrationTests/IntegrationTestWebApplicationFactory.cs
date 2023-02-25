using System.Net.Http.Headers;

namespace CompanyName.Api.IntegrationTests
{
    /// <inheritdoc />
    public class IntegrationTestWebApplicationFactory: IDisposable
    {
        public IntegrationTestWebApplicationFactory()
        {
            this.Server = new WebAppFactory();
            this.Client = this.Server.CreateClient();
            this.Client.Timeout = TimeSpan.FromMinutes(1);
            this.Client.BaseAddress = new Uri("https://localhost:5001/");
            this.Client.DefaultRequestHeaders.Accept.Clear();
            this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets the HttpClient created for the test server.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Gets the testing server.
        /// </summary>
        private WebAppFactory Server { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Client.Dispose();
            this.Server.Dispose();
        }
    }
}
