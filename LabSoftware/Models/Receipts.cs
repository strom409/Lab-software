using System;

namespace EasioCore.Models
{
    public class Receipts
    {
        public long ReceiptID { get; set; }
        public string ReceiptNo { get; set; }
        public long LedgerID { get; set; }
        public long SubAccountsID { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public decimal Cashin { get; set; }
        public string MethodOfAdjustment { get; set; }
        public long GeneralIncomeID { get; set; }
        public string ReceiptMode { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string Bank { get; set; }
        public string ReceiptRemarks { get; set; }
        public string UserName { get; set; }
        public string ReceiptYear { get; set; }
        public string ReceiptMonth { get; set; }
        public bool? ReceiptDeleted { get; set; }
        public bool? ChequeStatus { get; set; }
        public bool? DIDFK { get; set; }
        public string FYear { get; set; }
        public string DepositLedgerIDFK { get; set; }
        public bool? RefundStatus { get; set; }
        public string PatientIDFK { get; set; }
        public int RType { get; set; }
        public int BalanceType { get; set; }
        public int LabID { get; set; }
        public string RTime { get; set; }
        public string RBarCodePath { get; set; }
    }
}
