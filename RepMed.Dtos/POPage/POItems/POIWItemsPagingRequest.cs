using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RepMed.Dtos.POPage.POItems
{
    public class POIWItemsPagingRequest: PODWPagingRequest
    {
        [JsonProperty("status")]
        public List<string>? Status { get; set; }
        [JsonProperty("statusFilter")]
        public string? StatusFilter { get; set; }
        [Required]
        public long PharmacyId { get; set; }
        public long ProductId { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;
    }
}
