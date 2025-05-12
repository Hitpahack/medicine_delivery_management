using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseShortbookDto
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public long PharmacyId { get; set; }

        public long? SupplierId { get; set; }
        [Required]
        public long Quantity { get; set; }

        public string Priority { get; set; }

        public string Status { get; set; }

        public DateTime? AddedDate { get; set; }
    }
    public class EntityShortbookDto : BaseShortbookDto
    {
        public long Id { get; set; }
    }
}
