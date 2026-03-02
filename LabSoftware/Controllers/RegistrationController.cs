using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using EasioCore.BLL;
using EasioCore.Models;
using Newtonsoft.Json;

namespace LabSoftware.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public RegistrationController(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public IActionResult Index()
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");

            string userTypeId = HttpContext.Session.GetString("userTypeID");
            string confirmOtp = HttpContext.Session.GetString("ConfirmOTP");
            string otp = HttpContext.Session.GetString("OTP");

            if (string.IsNullOrEmpty(confirmOtp) || confirmOtp != otp)
                return RedirectToAction("Login", "Auth");

            if (userTypeId != "4")
                return RedirectToAction("Index", "Dashboard");

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName") ?? "Registration User";
            ViewBag.FYear = HttpContext.Session.GetString("FYear") ?? "2025-26";
            ViewBag.TodaysCollection = AccountBLL.GetTodaysCollectionForUser(HttpContext.Session.GetString("UserName")).ToString("N2");

            var hospitals = TestMasterBLL.GetListOfHospitals();
            ViewBag.Hospitals = hospitals;

            var districts = DistrictBLL.GetListOfDistricts();
            ViewBag.Districts = districts;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterPatient(
            string PatientName, string Parentage, string Gender, string MobileNumber, string Age, string AgeType,
            string Address, string DistrictId, string Date, string MrdOpdNo, string HIDFK)
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");

            string userTypeId = HttpContext.Session.GetString("userTypeID");
            if (userTypeId != "4")
                return RedirectToAction("Index", "Dashboard");

            var patient = new SMHSData
            {
                ActionType = 1, 
                MRDNo = MrdOpdNo ?? "",
                PatientType = "0",
                PatientName = PatientName ?? "",
                Age = Age ?? "",
                Gender = Gender ?? "",
                PatientAddress = Address ?? "",
                MobileNo = MobileNumber ?? "",
                RegistrationDate = DateBLL.changeDateToMMDDYYYY(Date ?? ""),
                UserName = HttpContext.Session.GetString("UserName") ?? "",
                AgeType = AgeType ?? "Years",
                DistrictName = DistrictId ?? "",
                Parentage = Parentage ?? "",
                PatientIDFK = "",
                PatientCategory = "",
                SMDID = "1",
                MedicalUnitFK = "0",
                DistrictId = DistrictId ?? "",
                PatientTypeName = "",
                UID = "",
                HIDFK = HIDFK ?? "0"
            };

            Response response = SMHSBLL.SubmitPatientByMRDID(patient);
            if (response.ResponseIsSuccess())
            {
                TempData["MsgType"] = "success";
                TempData["MsgAlert"] = response.Message ?? "Patient registered successfully.";
                if (response.Status == 1 && response.ResponseData != null)
                {
                    var patientReg = JsonConvert.DeserializeObject<SMHSData>(response.ResponseData.ToString());
                    if (patientReg != null)
                    {
                        HttpContext.Session.SetString("UID", patientReg.UID ?? "");
                        HttpContext.Session.SetString("PatientID", patientReg.PatientID ?? "");
                        HttpContext.Session.SetString("MrdNo", MrdOpdNo ?? "");
                    }
                }
                return RedirectToAction("PatientRegistration");
            }
            if (response.ResponseIsWarning())
            {
                TempData["MsgType"] = "warning";
                TempData["MsgAlert"] = response.Message ?? "Warning.";
                return RedirectToAction("Index");
            }
            TempData["MsgType"] = "error";
            TempData["MsgAlert"] = response.Message ?? "Error saving patient.";
            return RedirectToAction("Index");
        }

        public IActionResult PatientRegistration()
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");

            string uid = HttpContext.Session.GetString("UID");
            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("Index");

            var ufn = HttpContext.Session.GetString("UserFullName");
            var un = HttpContext.Session.GetString("UserName");
            ViewBag.UserFullName = (string.IsNullOrEmpty(ufn) || string.Equals(ufn, "Registration User", StringComparison.OrdinalIgnoreCase)) ? (un ?? "") : ufn;
            ViewBag.FYear = HttpContext.Session.GetString("FYear") ?? "2025-26";
            ViewBag.TodaysCollection = AccountBLL.GetTodaysCollectionForUser(HttpContext.Session.GetString("UserName")).ToString("N2");

            var response = SMHSBLL.getPatientsOnUID(uid);
            if (!response.ResponseIsSuccess() || response.ResponseData == null)
            {
                TempData["MsgType"] = "error";
                TempData["MsgAlert"] = response.Message ?? "Could not load patient.";
                return RedirectToAction("Index");
            }
            var patient = JsonConvert.DeserializeObject<SMHSData>(response.ResponseData.ToString());
            ViewBag.Patient = patient;

            var pt = "OTHER";
            if (int.TryParse(patient?.PatientType, out int ptNum) && Enum.IsDefined(typeof(PatientsTypes), ptNum))
                pt = Enum.GetName(typeof(PatientsTypes), ptNum);
            ViewBag.PatientTypeDisplay = pt;

            ViewBag.DateToday = DateTime.Now.ToString("dd-MM-yyyy");

            var catResponse = LabBLL.GetAllTestCategories();
            var tests = TestMasterBLL.GetAllTests();
            var allTestsList = new List<object>();
            if (catResponse.ResponseIsSuccess() && tests != null && tests.Count > 0)
            {
                foreach (var t in tests.OrderBy(x => x.TestName))
                    allTestsList.Add(new { Id = t.TestID + "|false", Name = t.TestName, Rate = t.Rate });
            }
            ViewBag.AllTestsList = allTestsList;

            bool showReceipt = Request.Query["showReceipt"].ToString() == "1" || (TempData["ShowReceipt"] as bool? == true);
            ViewBag.ShowReceipt = showReceipt;
            if (showReceipt)
            {
                ViewBag.ReceiptNo = HttpContext.Session.GetString("ReceiptNo");
                ViewBag.RecpRemarks = HttpContext.Session.GetString("RecpRemarks");
                ViewBag.ReceiptLines = SessionReceipt.GetReceiptLines(HttpContext.Session);
                ViewBag.ReceiptTotal = HttpContext.Session.GetString("ReceiptTotal");
                ViewBag.ReceiptPaymentMode = HttpContext.Session.GetString("ReceiptPaymentMode");
                ViewBag.ReceiptDate = HttpContext.Session.GetString("ReceiptDate");
                ViewBag.CompanyName = HTBLL.CompanyName();
                ViewBag.ReceiptSubtitle = _config.GetValue<string>("ReceiptSubtitle") ?? "";
                var receiptPatientJson = HttpContext.Session.GetString("ReceiptPatient");
                ViewBag.ReceiptPatient = string.IsNullOrEmpty(receiptPatientJson) ? null : JsonConvert.DeserializeObject<SMHSData>(receiptPatientJson);
                ViewBag.ReceiptFreeTypeName = HttpContext.Session.GetString("ReceiptFreeTypeName") ?? "Full Paid";
                ViewBag.ReceiptTransactionNo = HttpContext.Session.GetString("ReceiptTransactionNo") ?? "";
            }

            return View();
        }

        [HttpPost]
        public IActionResult SaveBill([FromBody] SaveBillRequest request)
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return Json(new { success = false, message = "Not logged in." });

            string uid = HttpContext.Session.GetString("UID");
            string patientId = HttpContext.Session.GetString("PatientID");
            string mrdNo = HttpContext.Session.GetString("MrdNo");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(patientId))
                return Json(new { success = false, message = "Patient session expired." });

            if (request?.Tests == null || request.Tests.Count == 0)
                return Json(new { success = false, message = "No tests selected." });

            var tests = TestMasterBLL.GetAllTests();
            var categories = new List<TestCategory>();
            var catResp = LabBLL.GetAllTestCategories();
            if (catResp.ResponseIsSuccess() && catResp.ResponseData != null)
                categories = JsonConvert.DeserializeObject<List<TestCategory>>(catResp.ResponseData.ToString()) ?? new List<TestCategory>();

            var ptDetailList = new List<PatientTestDetail>();
            var ptList = new List<PatientTestList>();
            decimal totalAmount = 0;
            var receiptLines = new List<ReceiptLine>();

            foreach (var t in request.Tests)
            {
                var testIdStr = t.TestId?.ToString().Split('|')[0];
                if (string.IsNullOrEmpty(testIdStr)) continue;
                long tid;
                if (!long.TryParse(testIdStr, out tid)) continue;
                var test = tests?.FirstOrDefault(x => x.TestID == tid);
                if (test == null) continue;

                decimal rate = test.Rate;
                decimal amount = t.Amount;
                decimal discount = rate - amount;
                totalAmount += amount;
                receiptLines.Add(new ReceiptLine { Name = test.TestName, Rate = rate, Discount = discount, Amount = amount });

                var cat = categories.FirstOrDefault(c => c.TestCategoryID == test.TestCategoryIDFK);
                ptDetailList.Add(new PatientTestDetail
                {
                    PatientTestDetailID = 0,
                    TestIDFK = test.TestID.ToString(),
                    TestCatIDFK = test.TestCategoryIDFK.ToString(),
                    MedicalDepartmentIDFK = test.MDIDFK.ToString(),
                    TestName = test.TestName,
                    Rate = rate,
                    Discount = discount,
                    Amount = amount.ToString(),
                    FYear = HttpContext.Session.GetString("FYear") ?? "",
                    SystemYear = DateTime.Now.ToString("yyyy"),
                    SystemMonth = DateTime.Now.ToString("MMMM"),
                    SampleTestDate = DateTime.Now.Date,
                    AnalysedDate = DateTime.Now.Date,
                    RecieptBy = HttpContext.Session.GetString("UserName") ?? "",
                    PatientIDFK = patientId,
                    Qnty = "1",
                    IsDeleted = false,
                    IsFree = amount == 0 ? "1" : "0",
                    LabIDFK = test.LabIDFK ?? "",
                    RTypeFK = test.RTypeIDFK ?? ""
                });

                if (cat != null && ptList.All(p => p.TestCategoryIDFK != cat.TestCategoryID.ToString()))
                {
                    ptList.Add(new PatientTestList
                    {
                        PTLID = 0,
                        PatientIDFK = patientId,
                        TestCategoryIDFK = cat.TestCategoryID.ToString(),
                        TestCategoryName = cat.TestCategoryName ?? "",
                        DateOfTest = DateTime.Now.Date,
                        ReadyDate = DateTime.Now.Date,
                        CashierID = HttpContext.Session.GetString("UserName") ?? "",
                        SampleTime = DateTime.Now.ToString("hh:mm:ss tt"),
                        Amount = amount,
                        AnalysedOn = DateTime.Now.Date,
                        ReportIssued = true,
                        LBID = cat.LabIDFK ?? "",
                        RType = cat.RType ?? "",
                        IsFree = amount == 0 ? "1" : "0"
                    });
                }
            }

            var patient = JsonConvert.DeserializeObject<SMHSData>(JsonConvert.SerializeObject(new SMHSData { PatientID = patientId, PatientName = "" }));
            var patientResponse = SMHSBLL.getPatientsOnUID(uid);
            if (patientResponse.ResponseIsSuccess() && patientResponse.ResponseData != null)
                patient = JsonConvert.DeserializeObject<SMHSData>(patientResponse.ResponseData.ToString());

            var pd = new PatientData
            {
                PID = patientId,
                UID = uid,
                ActionType = 0,
                SampleType = request.TransactionNo ?? "",
                FreeType = request.FreeType ?? "0",
                FreeTypeName = request.FreeType ?? "0",
                FreeAmount = request.TotalAmount?.ToString() ?? totalAmount.ToString(),
                Patient = new SMHSData { PatientID = patientId, PatientName = patient?.PatientName ?? "" },
                Receipt = new Receipts
                {
                    LedgerID = 0,
                    SubAccountsID = 0,
                    Cashin = totalAmount,
                    ReceiptMode = request.PaymentMode ?? "Cash",
                    MethodOfAdjustment = request.PaymentMode ?? "Cash",
                    GeneralIncomeID = 0,
                    ReceiptRemarks = request.Remarks ?? "",
                    UserName = HttpContext.Session.GetString("UserName") ?? "",
                    ReceiptYear = DateTime.Now.ToString("yyyy"),
                    ReceiptMonth = DateTime.Now.ToString("MMMM"),
                    ReceiptDeleted = false,
                    ChequeStatus = false,
                    DIDFK = false,
                    FYear = HttpContext.Session.GetString("FYear") ?? "",
                    RefundStatus = false,
                    PatientIDFK = patientId,
                    RType = 1,
                    BalanceType = 0,
                    LabID = 0,
                    ChequeDate = DateTime.Now.Date,
                    ReceiptDate = DateTime.Now.Date,
                    RTime = DateTime.Now.ToString("hh:mm tt")
                },
                PTL = ptList,
                PTDetail = ptDetailList
            };

            var result = PatientBLL.postPatientData(pd);
            if (!result.IsSuccess)
                return Json(new { success = false, message = result.Message ?? "Failed to save bill." });
            if (result.Status != 1)
                return Json(new { success = false, message = result.Message ?? "Warning.", isWarning = true });

            HttpContext.Session.SetString("ReceiptNo", result.ReceiptNo ?? "");
            HttpContext.Session.SetString("RecpRemarks", request.Remarks ?? "");
            HttpContext.Session.SetString("ReceiptTotal", totalAmount.ToString());
            HttpContext.Session.SetString("ReceiptPaymentMode", request.PaymentMode ?? "Cash");
            HttpContext.Session.SetString("ReceiptDate", DateTime.Now.ToString("dd-MM-yyyy hh:mm tt"));
            SessionReceipt.SetReceiptLines(HttpContext.Session, receiptLines);
            HttpContext.Session.SetString("ReceiptPatient", JsonConvert.SerializeObject(patient));
            var freeTypeName = request.FreeType == "0" ? "Full Paid" : request.FreeType == "1" ? "25% Discount" : request.FreeType == "2" ? "50% Discount" : request.FreeType == "3" ? "75% Discount" : "Free";
            HttpContext.Session.SetString("ReceiptFreeTypeName", freeTypeName);
            HttpContext.Session.SetString("ReceiptTransactionNo", request.TransactionNo ?? "");

            // Generate QR code for receipt (same as Anantnag rpReciept_ItemDataBound)
            var qrText = "UID: " + patient?.UID + ", Name: " + (patient?.PatientName ?? "") + ", Amount: " + totalAmount + ", Date: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");
            QRCodeHelper.GenerateQRCodeReceipt(qrText, patient?.UID ?? uid, _env.WebRootPath);

            return Json(new { success = true, message = "Reciept Saved Successfully.", redirect = Url.Action("PatientRegistration", "Registration", new { showReceipt = 1 }) });
        }

        public IActionResult DuplicateReceipt(string searchBy, string searchValue, string dateFrom, string dateTo)
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");
            if (HttpContext.Session.GetString("userTypeID") != "4")
                return RedirectToAction("Index", "Dashboard");

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName") ?? "Registration User";
            ViewBag.FYear = HttpContext.Session.GetString("FYear") ?? "2025-26";
            ViewBag.TodaysCollection = AccountBLL.GetTodaysCollectionForUser(HttpContext.Session.GetString("UserName")).ToString("N2");

            ViewBag.SearchBy = searchBy ?? "-1";
            ViewBag.SearchValue = searchValue ?? "";
            ViewBag.DateFrom = dateFrom ?? DateBLL.getDateTimeNow();
            ViewBag.DateTo = dateTo ?? DateBLL.getDateTimeNow();

            var receipts = new List<DupReceipt>();
            string userName = HttpContext.Session.GetString("UserName") ?? "";

            if (!string.IsNullOrEmpty(searchBy) && searchBy != "-1")
            {
                Response response = null;
                if (searchBy == "0")
                    response = ReceiptBLL.GetRecieptOnRecieptNo(searchValue);
                else if (searchBy == "1")
                {
                    var param = DateBLL.changeDateToMMDDYYYY(dateFrom ?? "") + "," + DateBLL.changeDateToMMDDYYYY(dateTo ?? "") + ",All";
                    response = ReceiptBLL.GetRecieptsFromDateToDate(param);
                }
                else if (searchBy == "2")
                    response = ReceiptBLL.GetRecieptOnPatientUID(searchValue);

                if (response != null && response.ResponseIsSuccess() && response.ResponseData != null)
                {
                    receipts = JsonConvert.DeserializeObject<List<DupReceipt>>(response.ResponseData.ToString()) ?? new List<DupReceipt>();
                    receipts = receipts.Where(x => (x.UserName ?? "") == userName).OrderByDescending(x => long.TryParse(x.ReceiptID, out var id) ? id : 0).ToList();
                }
                else if (response != null && response.ResponseIsWarning())
                    TempData["MsgType"] = "warning";
                else if (response != null && response.ResponseIsError())
                    TempData["MsgType"] = "error";
            }

            ViewBag.Receipts = receipts;
            return View();
        }

        public IActionResult DuplicateReceiptPrint(string rid)
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");
            if (string.IsNullOrEmpty(rid))
                return RedirectToAction("DuplicateReceipt");

            var pd = ReceiptBLL.GetDuplicateReceiptOnRID(rid);
            if (pd == null || pd.Receipt == null)
            {
                TempData["MsgType"] = "error";
                TempData["MsgAlert"] = "Receipt not found.";
                return RedirectToAction("DuplicateReceipt");
            }
            ViewBag.PatientData = pd;
            ViewBag.CompanyName = HTBLL.CompanyName();
            return View();
        }

        public IActionResult TodaysCollection(string dateFrom, string dateTo)
        {
            if (!LoginBLL.IsLoggedIn(HttpContext.Session))
                return RedirectToAction("Login", "Auth");
            if (HttpContext.Session.GetString("userTypeID") != "4")
                return RedirectToAction("Index", "Dashboard");

            ViewBag.UserFullName = HttpContext.Session.GetString("UserFullName") ?? "Registration User";
            ViewBag.FYear = HttpContext.Session.GetString("FYear") ?? "2025-26";
            ViewBag.TodaysCollection = AccountBLL.GetTodaysCollectionForUser(HttpContext.Session.GetString("UserName")).ToString("N2");

            string from = dateFrom ?? DateBLL.getDateTimeNow();
            string to = dateTo ?? DateBLL.getDateTimeNow();
            ViewBag.DateFrom = from;
            ViewBag.DateTo = to;
            ViewBag.ForUser = HttpContext.Session.GetString("UserName") ?? "";

            var receipts = new List<DupReceipt>();
            string userName = HttpContext.Session.GetString("UserName") ?? "";
            string param = DateBLL.changeDateToMMDDYYYY(from) + "," + DateBLL.changeDateToMMDDYYYY(to) + "," + userName;
            var response = AccountBLL.GetRecieptsForUser(param);
            if (response.ResponseIsSuccess() && response.ResponseData != null)
            {
                receipts = JsonConvert.DeserializeObject<List<DupReceipt>>(response.ResponseData.ToString()) ?? new List<DupReceipt>();
                receipts = receipts.OrderByDescending(x => long.TryParse(x.ReceiptID, out var id) ? id : 0).ToList();
            }
            ViewBag.Receipts = receipts;
            decimal totalCash = receipts.Where(r => string.Equals(r.ReceiptMode, "Cash", StringComparison.OrdinalIgnoreCase)).Sum(r => decimal.TryParse(r.Amt, out var a) ? a : 0);
            decimal totalTransfer = receipts.Where(r => string.Equals(r.ReceiptMode, "Transfer", StringComparison.OrdinalIgnoreCase)).Sum(r => decimal.TryParse(r.Amt, out var a) ? a : 0);
            ViewBag.TotalCash = totalCash;
            ViewBag.TotalTransfer = totalTransfer;
            ViewBag.Total = receipts.Sum(r => decimal.TryParse(r.Amt, out var a) ? a : 0);
            ViewBag.CompanyName = HTBLL.CompanyName();
            return View();
        }
    }

    public class SaveBillRequest
    {
        public List<SaveBillTestItem> Tests { get; set; }
        public string FreeType { get; set; }
        public string PaymentMode { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string TransactionNo { get; set; }
    }

    public class SaveBillTestItem
    {
        public string TestId { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }

    public class ReceiptLine
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
    }

    public static class SessionReceipt
    {
        const string Key = "ReceiptLinesJson";
        public static void SetReceiptLines(ISession session, List<ReceiptLine> lines)
        {
            session.SetString(Key, JsonConvert.SerializeObject(lines ?? new List<ReceiptLine>()));
        }
        public static List<ReceiptLine> GetReceiptLines(ISession session)
        {
            var json = session.GetString(Key);
            if (string.IsNullOrEmpty(json)) return new List<ReceiptLine>();
            return JsonConvert.DeserializeObject<List<ReceiptLine>>(json) ?? new List<ReceiptLine>();
        }
    }
}
