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

        public Guid CustomerId { get; set; }
        public string Name { get; set; } = null!;

        public string? Domain { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

    }
}
