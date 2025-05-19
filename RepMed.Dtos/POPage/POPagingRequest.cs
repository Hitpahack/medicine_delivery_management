using Newtonsoft.Json;
using RepMed.Dtos.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage
{
    public class POPagingRequest : PagingRequest
    {
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("statusFilter")]
        public string? StatusFilter { get; set; }
        public long PharmacyId { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;
        public DateTime? FromDate { get; set; }  
        public DateTime? ToDate { get; set; }
    }
}
