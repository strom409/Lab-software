using System.Collections.Generic;
using System.Net.Http;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class LabBLL
    {
        public static Response GetAllTestCategories()
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("testcategory/4|0").Result;
                var json = msg.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Response>(json) ?? new Response { IsSuccess = false };
            }
            catch (System.Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
