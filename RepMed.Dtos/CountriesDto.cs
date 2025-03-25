using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class BasicCountriesDto
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public bool? IsEnable { get; set; }
    }

    public class EntityCountriesDto : BasicCountriesDto
    {
        public int Id { get; set; }
        public string TimeZone { get; set; }
        public string CountryCodeTwo { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
    }

    
}
