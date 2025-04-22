using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.ProductPage
{
    public class ProductsPagingResponse : SP_BASE_RESPONSE
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }

    }
}
