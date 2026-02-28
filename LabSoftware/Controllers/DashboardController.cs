using Microsoft.AspNetCore.Mvc;
using EasioCore.BLL;
using EasioCore.Models;
using Newtonsoft.Json;

namespace LabSoftware.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Same logic as AnantnagRevenueUI DashBoard.aspx
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");

            string confirmOtp = HttpContext.Session.GetString("ConfirmOTP");
            string otp = HttpContext.Session.GetString("OTP");

            if (string.IsNullOrEmpty(confirmOtp) || confirmOtp != otp)
                return RedirectToAction("Login", "Auth");

            // Allow all user types (1–6) to access dashboard; Registration/Labs/Accounts not in MVC yet

            // Load menu and dashboard data (same as AnantnagRevenueUI)
            string masterIds = HttpContext.Session.GetString("MasterIDs") ?? "";
            ViewBag.LeftMenu = DashBoardBLL.LeftMenuList(masterIds);
            ViewBag.ActiveUsers = DashBoardBLL.ActiveUserList();
            ViewBag.UserMessages = DashBoardBLL.UserMessageBoxList();
            ViewBag.Notices = DashBoardBLL.DashBoardNoticeList();

            // Dashboard stats from API
            var dash = new DashDetails();
            var response = DashBLLAPI.GetDataForDashBoard();
            if (response.IsSuccess && response.Status == 1 && response.ResponseData != null)
            {
                var jsonString = response.ResponseData.ToString();
                dash = JsonConvert.DeserializeObject<DashDetails>(jsonString) ?? new DashDetails();
            }
            ViewBag.DashDetails = dash;

            return View();
        }
    }
}
