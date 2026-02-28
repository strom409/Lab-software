namespace EasioCore.BLL
{
    public static class DateBLL
    {
        /// <summary>Returns current date in dd-MM-yyyy format (same as Anantnag).</summary>
        public static string getDateTimeNow()
        {
            return System.DateTime.Now.ToString("dd-MM-yyyy");
        }

        /// <summary>Converts dd-MM-yyyy to MM-dd-yyyy (API format).</summary>
        public static string changeDateToMMDDYYYY(string dateDDMMYYYY)
        {
            if (string.IsNullOrWhiteSpace(dateDDMMYYYY)) return "";
            string[] dd = dateDDMMYYYY.Trim().Split('-');
            if (dd.Length != 3) return dateDDMMYYYY;
            string strDD = dd[0];
            string strMM = dd[1];
            string strYYYY = dd[2];
            return strMM + "-" + strDD + "-" + strYYYY;
        }
    }
}
