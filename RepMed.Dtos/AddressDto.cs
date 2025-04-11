using RepMed.Core;
using System;

namespace RepMed.Dtos
{
    public class BasicAddressDto
    {
        [Required]
        public string AddressLine { get; set; }
        [Required]

        public int CityId { get; set; }
        [Required]

        public int StateId { get; set; }
        [Required]

        public int CountryId { get; set; }
        [Required]

        public string Pincode { get; set; }
        [Required]

        public decimal? Latitude { get; set; }
        [Required]
        public decimal? Longitude { get; set; }
    }

    public class EntityAddressDto 
    {
        public int Id { get; set; }

        public int UserId { get; set; }

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
        public long PersonId { get; set; }

    }
}
