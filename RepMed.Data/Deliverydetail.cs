using System;
using System.Collections.Generic;

namespace RepMed.Data;

public partial class Deliverydetail
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long DeliveryPersonId { get; set; }

    public string DeliveryStatus { get; set; }

    public DateTime? EstimatedDelivery { get; set; }

    public DateTime? ActualDeliveryTime { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public bool? IsCod { get; set; }

    public bool? Codcollected { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Deliveryperson DeliveryPerson { get; set; }

    public virtual Order Order { get; set; }
}
