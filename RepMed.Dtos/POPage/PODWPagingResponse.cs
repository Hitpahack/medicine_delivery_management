using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.POPage
{
    public class PODWPagingResponse :SP_BASE_RESPONSE
    {
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
