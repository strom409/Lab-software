using System.Collections.Generic;
using System.Net.Http;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class DistrictBLL
    {
        public static List<District> GetListOfDistricts()
        {
            var list = new List<District>();
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("district").Result;
                var json = msg.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<Response>(json);
                if (response != null && response.IsSuccess && response.Status == 1 && response.ResponseData != null)
                {
                    var dataStr = response.ResponseData.ToString();
                    list = JsonConvert.DeserializeObject<List<District>>(dataStr) ?? new List<District>();
                }
            }
            catch { }
            return list;
        }
    }
}
