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
        public int ExpiredSubscriptionCount { get; set; }
        public int SuspendedSubscriptionCount { get; set; }
        public int DeletedSubscriptionCount { get; set; }
        public int DisabledSubscriptionCount { get; set; }

        public int AutoRenew { get; set; }
        public int Unused { get; set; }
        public int Renewal { get; set; }

    }
}
