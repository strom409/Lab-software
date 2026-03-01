using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EasioCore.BLL
{
    /// <summary>
    /// API client for NITS. BaseUrl is set from appsettings (APIURL) in Program.cs at startup.
    /// </summary>
    public class NitsAPI
    {
        /// <summary>Set from configuration (APIURL) in Program.cs. Do not hardcode here.</summary>
        public static string BaseUrl { get; set; } = string.Empty;

        public static HttpClient apiConnection()
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
                throw new InvalidOperationException("NitsAPI.BaseUrl is not set. Ensure APIURL is configured in appsettings.json and Program.cs sets NitsAPI.BaseUrl at startup.");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
