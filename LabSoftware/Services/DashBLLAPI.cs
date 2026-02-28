using System.Collections.Generic;
using System.Net.Http;
using EasioCore.Models;

namespace EasioCore.BLL
{
    public class DashBLLAPI
    {
        public static Response GetDataForDashBoard()
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("dashicons/0|0").Result;
                return msg.Content.ReadAsAsync<Response>().Result;
            }
            catch
            {
                return new Response { IsSuccess = false, Status = 0 };
            }
        }
    }
}
