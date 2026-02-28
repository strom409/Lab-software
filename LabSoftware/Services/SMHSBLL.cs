using System.Net.Http;
using System.Threading.Tasks;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class SMHSBLL
    {
        public static Response SubmitPatientByMRDID(SMHSData patient)
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var json = JsonConvert.SerializeObject(patient);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var msg = hc.PostAsync("smhs/", content).Result;
                var responseJson = msg.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Response>(responseJson) ?? new Response { IsSuccess = false, Message = "Invalid response." };
            }
            catch (System.Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }

        public static Response getPatientsOnUID(string uid)
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("smhs/1|" + uid).Result;
                var responseJson = msg.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Response>(responseJson) ?? new Response { IsSuccess = false };
            }
            catch (System.Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
