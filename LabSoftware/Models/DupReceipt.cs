using System.Collections.Generic;

namespace EasioCore.Models
{
    public class DupReceipt
    {
        public string PatientID { get; set; }
        public string PID { get; set; }
        public string PName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string RefferedBy { get; set; }
        public string Age { get; set; }
        public string AgeUnit { get; set; }
        public string Gender { get; set; }
        public string RegTime { get; set; }
        public string ReceiptNo { get; set; }
        public string ReceiptID { get; set; }
        public string Amt { get; set; }
        public string ReceiptDate { get; set; }
        public string ReceiptMode { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public string RType { get; set; }
        public string GIID { get; set; }
        public string TotalBillAmount { get; set; }
        public string Discount { get; set; }
        public string BalanceAmount { get; set; }
        public string AmountPaid { get; set; }
        public string Payable { get; set; }
        public string IsCancelled { get; set; }
        public List<PatientTestDetail> PTD { get; set; }
        public string MDIDFK { get; set; }
        public string BalanceType { get; set; }
        public string IsPaid { get; set; }
    }
}
