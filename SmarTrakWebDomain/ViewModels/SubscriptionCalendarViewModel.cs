using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.ViewModels
{
    public class SubscriptionCalendarViewModel
    {
        public string Month { get; set; } = "";
        public int Year { get; set; }
        public int Expiring { get; set; }
    }
}
