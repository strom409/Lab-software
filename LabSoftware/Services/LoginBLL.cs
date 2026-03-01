using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using EasioCore.Models;
using Newtonsoft.Json;

namespace EasioCore.BLL
{
    public class LoginBLL
    {
        public static string login(User Lmodel, ISession session)
        {
            return loginCredential(Lmodel, session);
        }

        public static string loginCredential(User Lmodel, ISession session)
        {
            string url = "0";
            User UModel = new User();
            
            try 
            {
                HttpClient http = NitsAPI.apiConnection();
                
                // Match Anantnag exactly: PostAsJsonAsync sends full User object
                var json = JsonConvert.SerializeObject(Lmodel, new JsonSerializerSettings 
                { 
                    NullValueHandling = NullValueHandling.Ignore 
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var responseTask = http.PostAsync("Login", content);
                responseTask.Wait();
                HttpResponseMessage Response = responseTask.Result;
                
                if (Response.IsSuccessStatusCode)
                {
                    var readTask = Response.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var jsonResponse = readTask.Result;
                    UModel = JsonConvert.DeserializeObject<User>(jsonResponse);
                }
                else
                {
                    // API returned error (500, 401, etc.) - return "error" so controller can show appropriate message
                    return "error:" + (int)Response.StatusCode;
                }
            } 
            catch (Exception)
            {
                return "error:0";
            }

            if (UModel != null && UModel.UserName != null)
            {
                session.SetString("Status", "LoggedIn");
                session.SetString("UID", UModel.UserID?.ToString() ?? "");
                session.SetString("userTypeID", UModel.userTypeID?.ToString() ?? "");
                session.SetString("UserFullName", UModel.userFullName ?? "");
                session.SetString("userLogoPath", UModel.userLogoPath ?? "");
                session.SetString("MasterIDs", UModel.MasterIDs ?? "");
                session.SetString("PageIDs", UModel.PageIDs ?? "");
                session.SetString("userEmail", UModel.userEmail ?? "");

                session.SetString("LabID", UModel.LabIDFK ?? "");
                session.SetString("DoctorID", UModel.DoctorIDFK ?? "");

                // FORE FEES ONLY
                session.SetString("Current_Session", UModel.current_Session ?? "");
                session.SetString("FYear", HTBLL.GetFyear());

                session.SetString("OTP", UModel.OTP ?? "");
                session.SetString("UserName", UModel.UserName ?? "");

                switch (UModel.userTypeID)
                {
                    case 1: url = "/Dashboard/Index"; break;
                    case 2: url = "/Dashboard/Index"; break;  // Labs not in MVC yet
                    case 3: url = "/Dashboard/Index"; break;  // Labs not in MVC yet
                    case 4: url = "/Registration/Index"; break;
                    case 5: url = "/Dashboard/Index"; break;
                    case 6: url = "/Dashboard/Index"; break;  // Accounts not in MVC yet
                    default: url = "/Dashboard/Index"; break;
                }
            }
           
            return url;
        }

        public static void LogoutUser(ISession session)
        {
            session.Clear();
        }
        
        public static bool IsLoggedIn(ISession session)
        {
            try
            {
                return session.GetString("Status") == "LoggedIn" && !string.IsNullOrEmpty(session.GetString("UserName"));
            }
            catch
            {
                return false;
            }
        }
    }
}
