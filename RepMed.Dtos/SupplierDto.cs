using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseSupplierDto
    {
        [Required]
        public long PharmacyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }

        public string Gstnumber { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
    public class EntitySupplierDto : BaseSupplierDto
    {
        public long Id { get; set; }

    }
}
