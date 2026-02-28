using System;
using System.Collections.Generic;
using System.Linq;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public static class AccountBLL
    {
        /// <summary>Param: DateFrom,DateTo,username (MM-dd-yyyy format). API: receipt/5|param</summary>
        public static Response GetRecieptsForUser(string param)
        {
            try
            {
                var hc = NitsAPI.apiConnection();
                var msg = hc.GetAsync("receipt/5|" + param).Result;
                return msg.Content.ReadAsAsync<Response>().Result;
            }
            catch
            {
                return new Response { IsSuccess = false, Message = "Request failed.", Status = 0 };
            }
        }

        /// <summary>Returns today's collection total for the current user (same as Anantnag MasterRegistration.CalculateCashCollectedToday).</summary>
        public static decimal GetTodaysCollectionForUser(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return 0;
            try
            {
                string today = DateBLL.getDateTimeNow();
                string mmddyyyy = DateBLL.changeDateToMMDDYYYY(today);
                string param = mmddyyyy + "," + mmddyyyy + "," + userName;
                Response response = GetRecieptsForUser(param);
                if (!response.ResponseIsSuccess() || response.ResponseData == null)
                    return 0;
                var jsonString = response.ResponseData.ToString();
                var receipts = JsonConvert.DeserializeObject<List<DupReceipt>>(jsonString);
                if (receipts == null || receipts.Count == 0) return 0;
                return receipts.Sum(x => decimal.TryParse(x.Amt, out var amt) ? amt : 0);
            }
            catch
            {
                return 0;
            }
        }
    }
}
