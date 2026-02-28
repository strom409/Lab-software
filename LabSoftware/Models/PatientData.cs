using System;
using System.Collections.Generic;

namespace EasioCore.Models
{
    public class PatientData
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public string PID { get; set; }
        public string UID { get; set; }
        public string RBarCodePath { get; set; }
        public int ActionType { get; set; }
        public string ReceiptNo { get; set; }
        public SMHSData Patient { get; set; }
        public Receipts Receipt { get; set; }
        public List<PatientTestList> PTL { get; set; }
        public List<PatientTestDetail> PTDetail { get; set; }
        public string SampleType { get; set; }
        public string FreeType { get; set; }
        public string FreeAmount { get; set; }
        public string FreeTypeName { get; set; }
    }
}
