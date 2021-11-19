using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Extensions
{
    public static class HttpClientExtensions
    {
#if !NETCOREAPP2_2
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T payload)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return client.PostAsync(requestUri, content);
        }
#endif
    }
}