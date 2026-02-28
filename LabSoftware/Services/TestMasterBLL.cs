using System.Collections.Generic;
using System.Net.Http;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class TestMasterBLL
    {
        public static List<Hospital> GetListOfHospitals()
        {
            var list = new List<Hospital>();
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("hospital").Result;
                var json = msg.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<Response>(json);
                if (response != null && response.IsSuccess && response.Status == 1 && response.ResponseData != null)
                {
                    var dataStr = response.ResponseData.ToString();
                    list = JsonConvert.DeserializeObject<List<Hospital>>(dataStr) ?? new List<Hospital>();
                }
            }
            catch { }
            return list;
        }

        public static List<Test> GetAllTests()
        {
            var list = new List<Test>();
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("tests/4|0").Result;
                var json = msg.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<Response>(json);
                if (response != null && response.IsSuccess && response.Status == 1 && response.ResponseData != null)
                {
                    var dataStr = response.ResponseData.ToString();
                    list = JsonConvert.DeserializeObject<List<Test>>(dataStr) ?? new List<Test>();
                }
            }
            catch { }
            return list;
        }
    }
}
