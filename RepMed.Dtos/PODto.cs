using AutoMapper.Configuration.Annotations;
using RepMed.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePODto
    {
        [Required]
        public long PharmacyId { get; set; }
        [Required]
        public long SupplierId { get; set; }
        public string Ponumber { get; set; }
        [JsonIgnore]
        public DateTime OrderDate { get; set; }
        public DateTime? Eddate { get; set; }
        [JsonIgnore]
        public string Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string Remarks { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
    }
    public class EntityPODto : BasePODto
    {
        public long Id { get; set; }
    }

    public class CreatePODto : BasePODto
    {
        [Required]
        public List<long> ShortbookId { get; set; }
    }
    public class NextPoNumberDto
    {
        public string NextPONumber { get; set; }
    }
}
