using RepMed.Core;
using Microsoft.AspNetCore.Http;
using System;


namespace RepMed.Dtos
{

    public class AddProvidersDtos : BasePersonDto
    {
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        //[StandardPassword]
        [IgnoreDapper]
        public string Password { get; set; }
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        //[StandardPassword]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [IgnoreDapper]
        public string ConfirmPassword { get; set; }
        [Required]
        public string SSN { get; set; }
        [Required]
        public Guid? CreatedBy { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int providerCatId { get; set; }

        [IgnoreDapper]
        public AddContactsDto Contact { get; set; }
        [IgnoreDapper]
        public UserCustomFieldsDtos[] CustomFields { get; set; }
    }

    public class EntityProvidersDtos : EntityPersonsDto
    {
        public Guid ProviderID { get; set; }
        public int providerCatId { get; set; }
        public UserCustomFieldsDtos[] CustomFields { get; set; }
    }

    public class ProvidersDtos 
    {
        public Guid ID { get; set; }
        public Guid User_Id { get; set; }
        public int providerCatId { get; set; }
    }


    public class CredentialDocuments
    {
        public Guid Id { get; set; }

    }
    public class ProviderPersonalInfo : BasePersonDto
    {
        public int Age { get; set; }
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        //[StandardPassword]
        [IgnoreDapper]
        public string Password { get; set; }
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        //[StandardPassword]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [IgnoreDapper]
        public string ConfirmPassword { get; set; }
        [Required]
        public string SSN { get; set; }
        [Required]
        public IFormFile ProfilePitcure { get; set; }
    }
    public class AddProvidersStepsDtos : BasePersonDto
    {

        public ProviderPersonalInfo PersonalInfo { get; set; }
        public AddContactsDto Contact { get; set; }
    }
}

