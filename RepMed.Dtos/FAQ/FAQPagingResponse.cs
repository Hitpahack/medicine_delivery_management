using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.FAQ
{
    public class FAQPagingResponse : SP_BASE_RESPONSE
    {
        public int Id { get; set; }
        public string Question { get; set; } 
        public string Answer { get; set; } 
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
