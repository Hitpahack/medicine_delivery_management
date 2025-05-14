using Newtonsoft.Json;
using RepMed.Dtos.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.ShortBookPage
{
    public class ShortbookPagingRequest : PagingRequest
    {
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("statusFilter")]
        public string? StatusFilter { get; set; }
        public long PharmacyId { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;

        public string? Date;
        
        
    }
}
