namespace EasioCore.Models
{
    public class SMHSData
    {
        public int ActionType { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PatientAddress { get; set; }
        public string MobileNo { get; set; }
        public string RegistrationDate { get; set; }
        public string UserName { get; set; }
        public string PatientCategory { get; set; }
        public string SMDID { get; set; }
        public string MedicalUnitFK { get; set; }
        public string AgeType { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string Parentage { get; set; }
        public string MRDNo { get; set; }
        public string PatientType { get; set; }
        public string PatientTypeName { get; set; }
        public string UID { get; set; }
        public string HIDFK { get; set; }
        public string PatientIDFK { get; set; }
        public string BRPathName { get; set; }
        public string PW { get; set; }
    }

    public enum PatientsTypes
    {
        OTHER = 0,
        OPD = 1,
        IPD = 2,
        SSA = 3
    }
}
