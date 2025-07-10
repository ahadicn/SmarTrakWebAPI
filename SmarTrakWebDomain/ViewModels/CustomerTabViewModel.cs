using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.ViewModels
{
    public class CustomerTabViewModel
    {
        public string Feature { get; set; } = "";
        public int Count { get; set; }
        public int Value { get; set; }
        public string? Billing { get; set; }
    }
}
