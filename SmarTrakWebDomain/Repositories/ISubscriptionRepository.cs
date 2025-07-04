using SmarTrakWebDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<PagedResult<SubscriptionModel>> GetAllSubscriptionsAsync(GetAllSubscriptionEntryModel parameter);
        Task<IEnumerable<SubscriptionModel>> GetByCustomerIdAsync(Guid customerId);
        Task<SPSubscriptionCountModel> GetSubscriptionCountAsync();
    }
}
