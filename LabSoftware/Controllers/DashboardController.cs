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
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");

            string confirmOtp = HttpContext.Session.GetString("ConfirmOTP");
            string otp = HttpContext.Session.GetString("OTP");

            if (string.IsNullOrEmpty(confirmOtp) || confirmOtp != otp)
                return RedirectToAction("Login", "Auth");


            string masterIds = HttpContext.Session.GetString("MasterIDs") ?? "";
            ViewBag.LeftMenu = DashBoardBLL.LeftMenuList(masterIds);
            ViewBag.ActiveUsers = DashBoardBLL.ActiveUserList();
            ViewBag.UserMessages = DashBoardBLL.UserMessageBoxList();
            ViewBag.Notices = DashBoardBLL.DashBoardNoticeList();

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
