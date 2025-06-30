using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Domain { get; set; }

    }
}
