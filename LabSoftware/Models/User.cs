namespace EasioCore.Models
{
    public class Login
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }

    public class User : Login
    {
        public long? UserID { get; set; }
        public string OldPassword { get; set; }
        public string userFullName { get; set; }
        public string userEmail { get; set; }
        public string userAddress { get; set; }
        public string userPhoneNo { get; set; }
        public string userTypeName { get; set; }
        public long? UserType { get; set; } //Not Used
        public long? userTypeID { get; set; }
        public string controlId { get; set; }
        public string userRemarks { get; set; }
        public string userLogoPath { get; set; }
        public string current_Session { get; set; }
        public string sessionID { get; set; }
        public string activation { get; set; }
        public string dashboard { get; set; }
        public string control1Id { get; set; }
        // For New UI
        public string MasterIDs { get; set; }
        public string PageIDs { get; set; }
        public string DIDFK { get; set; }
        public string LabIDFK { get; set; }
        public string DoctorIDFK { get; set; }

        public string UserPhoto { get; set; }
        public string OTP { get; set; }
    }
}
