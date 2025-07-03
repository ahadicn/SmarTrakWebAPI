using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Models
{
    public class SPSubscriptionCountModel
    {
        public int TotalSubscriptionCount { get; set; }
        public int ActiveSubscriptionCount { get; set; }        
        public int AutoRenewSubscriptionCount { get; set; }
        public int AutoRenewalSubscriptionCount { get; set; }
        public int ManualRenewalSubscriptionCount { get; set; }
    }
}
