using Newtonsoft.Json;
using RepMed.Dtos.DataTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage.POItems
{
    public class POOWItemsPagingRequest: PagingRequest
    {
        [JsonProperty("status")]
        public List<string>? Status { get; set; }
        [JsonProperty("statusFilter")]
        public string? StatusFilter { get; set; }
        [Required]
        public long PharmacyId { get; set; }
        public long POId { get; set; }
        public List<string>? StockAvailability { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
