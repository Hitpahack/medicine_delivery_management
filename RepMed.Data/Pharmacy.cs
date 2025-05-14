using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Pharmacy
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string StoreName { get; set; }

    public string BusinessName { get; set; }

    public string LicenseNumber { get; set; }

    public DateOnly? LicenseExpiry { get; set; }

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

    public virtual City City { get; set; }

    public virtual Country Country { get; set; }

    public virtual ICollection<Deliveryperson> Deliverypeople { get; set; } = new List<Deliveryperson>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Pharmacybankdetail> Pharmacybankdetails { get; set; } = new List<Pharmacybankdetail>();

    public virtual ICollection<Pharmacyinventory> Pharmacyinventories { get; set; } = new List<Pharmacyinventory>();

    public virtual ICollection<Pharmacystockledger> Pharmacystockledgers { get; set; } = new List<Pharmacystockledger>();

    public virtual ICollection<Purchaseinvoice> Purchaseinvoices { get; set; } = new List<Purchaseinvoice>();

    public virtual ICollection<Purchaseorder> Purchaseorders { get; set; } = new List<Purchaseorder>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();

    public virtual ICollection<Shortbook> Shortbooks { get; set; } = new List<Shortbook>();

    public virtual State State { get; set; }

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual User User { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
