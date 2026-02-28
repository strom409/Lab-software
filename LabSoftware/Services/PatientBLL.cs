using System.Net.Http;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class PatientBLL
    {
        public static PatientData postPatientData(PatientData pd)
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var json = JsonConvert.SerializeObject(pd);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var msg = hc.PostAsync("patient", content).Result;
                var responseJson = msg.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<PatientData>(responseJson) ?? new PatientData { IsSuccess = false };
            }
            catch (System.Exception ex)
            {
                return new PatientData { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
