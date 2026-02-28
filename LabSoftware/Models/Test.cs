using System;

namespace EasioCore.Models
{
    public class Test
    {
        public long TestID { get; set; }
        public string TestName { get; set; }
        public long TestCategoryIDFK { get; set; }
        public long TestgroupIDFK { get; set; }
        public long MDIDFK { get; set; }
        public string UnitName { get; set; }
        public string NormalRange { get; set; }
        public decimal Rate { get; set; }
        public bool? IsAvailable { get; set; }
        public string UserName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? MinRange { get; set; }
        public decimal? MaxRange { get; set; }
        public string ManualReport { get; set; }
        public string Shortcode { get; set; }
        public string Method { get; set; }
        public string ItemIDFK { get; set; }
        public string RTypeIDFK { get; set; }
        public int SortNo { get; set; }
        public string LabIDFK { get; set; }
    }
}
