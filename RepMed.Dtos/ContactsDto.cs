using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepMed.Dtos
{
    public class BasicContactsDto
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public int? CountryId { get; set; }
        [Required]
        public int? StateId { get; set; }
        [Required]
        public int? CityId { get; set; }
        [Required]
        public int? ZipCode { get; set; }
        public string Address1 { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string MobileNo { get; set; }
      


    }

    public class EntityContactsDto : BasicContactsDto
    {
        public Guid Id { get; set; }
        public string Fax { get; set; }
        public string MobileNo2 { get; set; }
        public string LandMark { get; set; }

        public virtual ICollection<BasicPersonsDto> Persons { get; set; }
    }

    public class AddContactsDto : BasicContactsDto
    {
        public Guid? PersonID { get; set; }

    }
}
