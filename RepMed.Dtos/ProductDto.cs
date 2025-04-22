using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class EntityProductDto
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal? Price { get; set; }

        public decimal? Mrp { get; set; }

        public decimal? Discount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
