using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseFAQDto
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
    public class EntityFAQDto: BaseFAQDto
    {
        public long Id { get; set; }
    }
}
