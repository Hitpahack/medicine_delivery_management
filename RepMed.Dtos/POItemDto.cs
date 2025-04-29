using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BasePOItemDto
    {
        [Required]
        public long PurchaseOrderId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        [Required]
        public string Unit { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; }
    }
    public class EntityPOItemDto: BasePOItemDto
    {
        public long Id { get; set; }        
    }
}
