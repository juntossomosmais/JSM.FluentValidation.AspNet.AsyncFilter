using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentValidation.AspNet.AsyncValidationFilter.Tests.Support.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadFromJsonAsync<T>(this HttpContent content) =>
            JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
    }
}