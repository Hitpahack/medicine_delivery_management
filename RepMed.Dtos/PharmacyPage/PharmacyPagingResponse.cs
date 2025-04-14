using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos.PharmacyPage
{
    public class PharmacyPagingResponse:SP_BASE_RESPONSE
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string RegisteredMobile { get; set; }
        public string OfficialEmail { get; set; }
        public string OwnerName { get; set; }
        public string Status { get; set; }
        public string Address1 { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}
