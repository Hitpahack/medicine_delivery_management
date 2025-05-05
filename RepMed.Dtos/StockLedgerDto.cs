using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMed.Dtos
{
    public class BaseStockLedger
    {
        public long PharmacyId { get; set; }

        public long ProductId { get; set; }

        public long PurchaseInvoiceId { get; set; }

        public long QuantityAdded { get; set; }

        public decimal SellingPriceAtTime { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
    public class EntityStockLedgerDto : BaseStockLedger
    {
        public long Id { get; set; }

    }
}
