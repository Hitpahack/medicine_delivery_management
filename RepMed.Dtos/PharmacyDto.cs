using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Dtos
{

    public class PharmacyBankDetailsDto
    {
        [JsonIgnore]
        public long Id { get; set; }
        [Required]
        public long PharmacyId { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string AccountHolderName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string Ifsccode { get; set; }
        [Required]
        public string BranchName { get; set; }

        public string UpiId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

    public class AddPharmacyDto
    {
        [JsonIgnore]
        public long Id { get; set; }

        public long UserId { get; set; }

        public string StoreName { get; set; }

        public string BusinessName { get; set; }

        public string LicenseNumber { get; set; }

        public DateTime? LicenseExpiry { get; set; }

        public string Gstnumber { get; set; }

        public string OwnerName { get; set; }

        public string RegisteredMobile { get; set; }

        public string OfficialEmail { get; set; }

        public string StoreMobile1 { get; set; }

        public string StoreEmail1 { get; set; }

        public string StoreEmail2 { get; set; }

        public bool? MobileVerified { get; set; }

        public bool? EmailVerified { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public long? CityId { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public bool? Otpverified { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

    }

    public class PharmacyDto: BasePharmacyDto
    {
        public AddPersonDto User { get; set; }
    }

    public class BasePharmacyDto
    {
        public AddPharmacyDto Pharmacy { get; set; }
        public PharmacyBankDetailsDto PharmacyBankDetails { get; set; }
    }
}
