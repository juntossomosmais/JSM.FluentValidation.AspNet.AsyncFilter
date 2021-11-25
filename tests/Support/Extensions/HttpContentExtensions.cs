using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace JSM.FluentValidation.AspNet.AsyncFilter.Tests.Support.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadFromJsonAsync<T>(this HttpContent content) =>
            JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());
    }
}