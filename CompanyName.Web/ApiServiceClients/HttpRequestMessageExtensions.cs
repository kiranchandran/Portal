using System.Net.Mime;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace CompanyName.Web.ApiServiceClients
{
    /// <summary>
    /// Extensions for <see cref="HttpRequestMessage"/>.
    /// </summary>
    internal static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Include JSON object with method.
        /// </summary>
        /// <typeparam name="T">Entity.</typeparam>
        /// <param name="request">HTTP request message.</param>
        /// <param name="requestParameter">JSON request object.</param>
        /// <returns>Updated request content.</returns>
        public static HttpRequestMessage WithJsonContent<T>(this HttpRequestMessage request, T requestParameter, bool ignoreNullProperties = true)
        {
            return WithJsonContent(request, requestParameter, MediaTypeNames.Application.Json, ignoreNullProperties);
        }

        /// <summary>
        /// Include JSON object with method.
        /// </summary>
        /// <typeparam name="T">Entity.</typeparam>
        /// <param name="request">HTTP request message.</param>
        /// <param name="requestParameter">JSON request object.</param>
        /// <param name="contentType">Content type.</param>
        /// <returns>Updated request content.</returns>
        public static HttpRequestMessage WithJsonContent<T>(this HttpRequestMessage request, T requestParameter, string contentType, bool ignoreNullProperties = true)
        {
            var json = JsonSerializer.Serialize<T>(requestParameter, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = ignoreNullProperties ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never
            });
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);
            request.Content = new StringContent(json, Encoding.UTF8, contentType);

            return request;
        }
    }
}
