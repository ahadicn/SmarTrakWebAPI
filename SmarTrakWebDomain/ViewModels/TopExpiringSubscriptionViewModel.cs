using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.ViewModels
{
    public class TopExpiringSubscriptionViewModel
    {
        public Guid SubscriptionId { get; set; }
        public string OfferName { get; set; } = "";
        public DateTime CommitmentEndDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
