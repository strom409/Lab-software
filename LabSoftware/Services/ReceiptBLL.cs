using System.Net.Http;
using EasioCore.Models;

namespace EasioCore.BLL
{
    public static class ReceiptBLL
    {
        /// <summary>Get receipt by receipt number. API: receipt/0|RNo</summary>
        public static Response GetRecieptOnRecieptNo(string rNo)
        {
            var hc = NitsAPI.apiConnection();
            var msg = hc.GetAsync("receipt/0|" + (rNo ?? "")).Result;
            return msg.Content.ReadAsAsync<Response>().Result;
        }

        /// <summary>Get receipts between dates. Param: DateFrom,DateTo,usernameOrAll (MM-dd-yyyy). API: receipt/1|param</summary>
        public static Response GetRecieptsFromDateToDate(string param)
        {
            var hc = NitsAPI.apiConnection();
            var msg = hc.GetAsync("receipt/1|" + (param ?? "")).Result;
            return msg.Content.ReadAsAsync<Response>().Result;
        }

        /// <summary>Get receipts by patient UID. API: receipt/4|UID</summary>
        public static Response GetRecieptOnPatientUID(string uid)
        {
            var hc = NitsAPI.apiConnection();
            var msg = hc.GetAsync("receipt/4|" + (uid ?? "")).Result;
            return msg.Content.ReadAsAsync<Response>().Result;
        }

        /// <summary>Get full receipt for duplicate print by ReceiptID. API: receipt/2|RID</summary>
        public static PatientData GetDuplicateReceiptOnRID(string rid)
        {
            var hc = NitsAPI.apiConnection();
            var msg = hc.GetAsync("receipt/2|" + (rid ?? "")).Result;
            return msg.Content.ReadAsAsync<PatientData>().Result;
        }
    }
}
