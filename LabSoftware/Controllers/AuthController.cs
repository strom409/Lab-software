using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EasioCore.BLL;
using EasioCore.Models;

namespace LabSoftware.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Remove("OTP");
            try { ViewBag.CompanyName = HTBLL.CompanyName(); } catch { ViewBag.CompanyName = "Lab Software"; }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName", "UserPassword")] User ur)
        {
            try
            {
                try { ViewBag.CompanyName = HTBLL.CompanyName(); } catch { ViewBag.CompanyName = "Lab Software"; }

                // Get values from form - binding can fail with complex models
                string userName = Request.Form["UserName"].ToString().Trim();
                string userPassword = Request.Form["UserPassword"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userPassword))
                {
                    ViewBag.Message = "Please enter User ID and Password.";
                    return View(new User { UserName = userName, UserPassword = userPassword });
                }

                var loginUser = new User { UserName = userName, UserPassword = userPassword };
                string rep = LoginBLL.login(loginUser, HttpContext.Session);

                if (rep.StartsWith("error:"))
                {
                    ViewBag.Message = rep == "error:500" 
                        ? "Service temporarily unavailable. Please try again later." 
                        : "Unable to connect to server. Please check your connection and try again.";
                    return View(loginUser);
                }

                if (rep == "0")
                {
                    ViewBag.Message = "Incorrect Username or Password";
                    return View(loginUser);
                }

                // Success: Store redirect URL and show OTP step (same logic as AnantnagRevenueUI)
                HttpContext.Session.SetString("responsefromAPI", rep);

                string userEmail = HttpContext.Session.GetString("userEmail");
                string otp = HttpContext.Session.GetString("OTP");
                string sessionUserName = HttpContext.Session.GetString("UserName");

                // Send OTP via email (same logic as AnantnagRevenueUI)
                if (!string.IsNullOrEmpty(userEmail) && EmailHelper.IsValidEmail(userEmail))
                {
                    string subject = "OTP For MMABM-AH-GMC MRD Revenue Software : " + otp;
                    string body = "Dear User: " + sessionUserName + ",<br /><br />Your OTP for E-Billing software is: " + otp + ", and is valid for 10 minutes only.<br /><br />Regards,<br />MMABM-AH-GMC MRD Section.";
                    string senderEmail = _config["Email:SenderEmail"] ?? "";
                    string senderPassword = _config["Email:SenderPassword"] ?? "";
                    if (!string.IsNullOrEmpty(senderEmail))
                        EmailHelper.SendMail(userEmail, subject, body, senderEmail, senderPassword);
                }
                else
                {
                    LoginBLL.LogoutUser(HttpContext.Session);
                    ViewBag.Message = "Invalid Email or Phone No!";
                    return View(loginUser);
                }

                ViewBag.Step = 1;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Login failed. Please try again. (" + ex.Message + ")";
                try { ViewBag.CompanyName = HTBLL.CompanyName(); } catch { ViewBag.CompanyName = "Lab Software"; }
                return View(ur ?? new User());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOTP(string confirmOtp)
        {
            string userName = HttpContext.Session.GetString("UserName");
            string otp = HttpContext.Session.GetString("OTP");
            string redirectUrl = HttpContext.Session.GetString("responsefromAPI");

            // Bypass OTP for test users (same as AnantnagRevenueUI)
            if (userName == "javeed" || userName == "nitsjk")
            {
                HttpContext.Session.SetString("ConfirmOTP", confirmOtp ?? "");
                return Redirect(redirectUrl ?? "/Dashboard/Index");
            }

            if (otp == confirmOtp)
            {
                HttpContext.Session.SetString("ConfirmOTP", confirmOtp);
                return Redirect(redirectUrl ?? "/Dashboard/Index");
            }

            ViewBag.Message = "Please Enter Valid OTP";
            ViewBag.Step = 1;
            ViewBag.CompanyName = HTBLL.CompanyName();
            return View("Login");
        }

        [HttpGet]
        public IActionResult BackToLogin()
        {
            LoginBLL.LogoutUser(HttpContext.Session);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            LoginBLL.LogoutUser(HttpContext.Session);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Error()
        {
            return RedirectToAction("Login");
        }
    }
}
