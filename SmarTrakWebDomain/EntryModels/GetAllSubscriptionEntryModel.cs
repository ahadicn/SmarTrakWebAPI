using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.EntryModels
{
    public class GetAllSubscriptionEntryModel
    {
        public string? SearchTerm { get; set; }
        public Guid? SearchCustomerId { get; set; }

        public string? Status { get; set; }
        public bool? AutoRenewal { get; set; }
                       

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc"; // or "desc"
    }
}
