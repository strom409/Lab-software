using System.Net;
using System.Net.Mail;

namespace EasioCore.BLL
{
    public class EmailHelper
    {
        public static string SendMail(string sendToEmail, string subject, string body, string senderEmail, string senderPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(sendToEmail) || !IsValidEmail(sendToEmail))
                    return "Invalid Email ID";

                using var mm = new MailMessage(senderEmail, sendToEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                using var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };
                smtp.Send(mm);
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
