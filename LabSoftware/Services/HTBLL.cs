using System.Net.Http;
using EasioCore.Models;

namespace EasioCore.BLL
{
    public class HTBLL
    {
        public static string GetFyear()
        {
            try
            {
                var ht = GetHtDetailsFromAPI();
                return ht?.FYear ?? "2024-2025";
            }
            catch
            {
                return "2024-2025";
            }
        }

        public static string CompanyName()
        {
            try
            {
                var ht = GetHtDetailsFromAPI();
                return ht?.Name ?? "Lab Software";
            }
            catch
            {
                return "Lab Software";
            }
        }

        public static HT GetHtDetailsFromAPI()
        {
            var hc = NitsAPI.apiConnection();
            var msg = hc.GetAsync("ht").Result;
            return msg.Content.ReadAsAsync<HT>().Result;
        }
    }
}
