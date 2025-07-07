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

        public string? Status { get; set; }
        public string? UnitType { get; set; }
        public string? BillingCycle { get; set; }
        public string? BillingType { get; set; }
        public bool? AutoRenewal { get; set; }
        public string? TermDuration { get; set; }
        public bool? IsTrial { get; set; }

        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public DateTime? StartedDateFrom { get; set; }
        public DateTime? StartedDateTo { get; set; }
        public DateTime? CommitmentEndDateFrom { get; set; }
        public DateTime? CommitmentEndDateTo { get; set; }
        public DateTime? EffectiveStartDateFrom { get; set; }
        public DateTime? EffectiveStartDateTo { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc"; // or "desc"
    }
}
