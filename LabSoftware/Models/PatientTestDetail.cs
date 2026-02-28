using System;

namespace EasioCore.Models
{
    public class PatientTestDetail
    {
        public long PatientTestDetailID { get; set; }
        public string TestIDFK { get; set; }
        public string TestCatIDFK { get; set; }
        public string MedicalDepartmentIDFK { get; set; }
        public string TestName { get; set; }
        public string CatName { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string FYear { get; set; }
        public string SystemYear { get; set; }
        public string SystemMonth { get; set; }
        public DateTime? SampleTestDate { get; set; }
        public DateTime? AnalysedDate { get; set; }
        public string RecieptBy { get; set; }
        public string AnalysedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string PatientIDFK { get; set; }
        public string Amount { get; set; }
        public string Qnty { get; set; }
        public string CIDFK { get; set; }
        public string LabIDFK { get; set; }
        public string RTypeFK { get; set; }
        public string IsFree { get; set; }
    }
}
