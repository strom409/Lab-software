using System;

namespace EasioCore.Models
{
    public class PatientTestList
    {
        public long PTLID { get; set; }
        public string PatientIDFK { get; set; }
        public string MDIDFK { get; set; }
        public string TestCategoryIDFK { get; set; }
        public string TestCategoryName { get; set; }
        public string TestGroupName { get; set; }
        public DateTime? DateOfTest { get; set; }
        public DateTime? ReadyDate { get; set; }
        public string ReferedByFK { get; set; }
        public string CashierID { get; set; }
        public string SampleTime { get; set; }
        public string CentreID { get; set; }
        public bool? IsCancelled { get; set; }
        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }
        public string AnalysedBy { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? AnalysedOn { get; set; }
        public bool? ReportIssued { get; set; }
        public string TGIDFK { get; set; }
        public string LBID { get; set; }
        public string RType { get; set; }
        public string IsFree { get; set; }
    }
}
