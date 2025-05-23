using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.SupplierPage
{
    public class SupplierPagingResponse : SP_BASE_RESPONSE
    {

        public long Id { get; set; }
        public string Name { get; set; }        
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string GSTNumber { get; set; }
        public string Status { get; set; }
    }
}
