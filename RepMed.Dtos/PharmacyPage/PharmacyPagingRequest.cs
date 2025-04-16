using Newtonsoft.Json;
using RepMed.Dtos.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.PharmacyPage
{
    public class PharmacyPagingRequest:PagingRequest
    {
        [JsonProperty("status")]    
        public string? Status { get; set; }
        [JsonProperty("statusFilter")]
        public string? StatusFilter { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;
        private string? date;
        public string? Date
        {
            get { return date; }
            set
            {
                if (!string.IsNullOrEmpty(value) && (value != "string" || value != "all"))
                    date = ParseYearMonth(value);
                else
                    date = "";
            }
        }
        internal string ParseYearMonth(string yearMonth)
        {
            // Ensure the input is valid (length 6, numeric)
            if (string.IsNullOrWhiteSpace(yearMonth) || yearMonth.Length != 6 || !int.TryParse(yearMonth, out int ym))
                throw new FormatException($"Invalid YearMonth format: {yearMonth}");

            // Extract Year and Month
            int year = ym / 100;  // First 4 digits are the year
            int month = ym % 100; // Last 2 digits are the month

            // Validate month range (1-12)
            if (month < 1 || month > 12)
                throw new FormatException($"Invalid month in YearMonth: {yearMonth}");

            return new DateTime(year, month, 1).ToString("yyyy-mm-dd"); // Set day to 1

        }
    }

}
