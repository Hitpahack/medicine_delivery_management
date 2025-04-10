using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Order
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public long PharmacyId { get; set; }

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; }

    public bool? IsDiffAddress { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string Pincode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public long? CityId { get; set; }

    public long? StateId { get; set; }

    public long? CountryId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual City City { get; set; }

    public virtual Country Country { get; set; }

    public virtual User Customer { get; set; }

    public virtual ICollection<Deliverydetail> Deliverydetails { get; set; } = new List<Deliverydetail>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Pharmacy Pharmacy { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual State State { get; set; }
}
