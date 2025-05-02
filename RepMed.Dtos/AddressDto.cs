using RepMed.Core;
using System;
using System.Text.Json.Serialization;

namespace RepMed.Dtos
{
    public class BasicAddressDto
    {
        public string AddressLine { get; set; }
            
        public long? CityId { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public string Pincode { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }

    public class EntityAddressDto 
    {
        public int Id { get; set; }
        public long PersonId { get; set; }

        public string AddressLine { get; set; }

        public int CityId { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        public string Pincode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

    }

    public class AddAddressDto : BasicAddressDto
    {
        [JsonIgnore]
        public long PersonId { get; set; }

    }
}
