using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Models
{
    public class CustomerSubscriptionModel
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }            

        public string OfferName { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int Quantity { get; set; }

        public string? UnitType { get; set; }

        public string BillingCycle { get; set; } = null!;

        public string BillingType { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public DateTime? StartedDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public bool? AutoRenewEnabled { get; set; }

        public string? TermDuration { get; set; }

        public DateTime? CommitmentEndDate { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        public bool? IsTrial { get; set; }

        public string? ProductId { get; set; }

        public string? SkuId { get; set; }
    }
}
