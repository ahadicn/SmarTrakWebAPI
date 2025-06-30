using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Models
{
    public class SubscriptionModel
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

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
              
    }
}
