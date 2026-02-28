using System;

namespace EasioCore.Models
{
    public class TestCategory
    {
        public long TestCategoryID { get; set; }
        public string TestCategoryName { get; set; }
        public string UserName { get; set; }
        public long TestgroupIDFK { get; set; }
        public string LastModifiedBy { get; set; }
        public string ManualReport { get; set; }
        public bool? IsDeleted { get; set; }
        public string InterP { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string MDIDFK { get; set; }
        public string TestList { get; set; }
        public string RType { get; set; }
        public string IsActive { get; set; }
        public string LabIDFK { get; set; }
        public int? IsPrimary { get; set; }
        public decimal? Rate { get; set; }
    }
}
