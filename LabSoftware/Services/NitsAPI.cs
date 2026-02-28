using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EasioCore.BLL
{
    public class NitsAPI
    {
        public static string BaseUrl { get; set; } = "https://dhanantnagapi.nitsjk.com/api/";

        public static HttpClient apiConnection()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
