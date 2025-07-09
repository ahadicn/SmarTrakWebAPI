using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.ViewModels;
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
        Task<IEnumerable<SubscriptionModel>> GetSubscriptionById(Guid Id);
        Task<SPSubscriptionCountModel> GetSubscriptionCountAsync();
        Task<List<SubscriptionCalendarViewModel>> GetSubscriptionCalendarAsync();
        Task<List<TopExpiringSubscriptionViewModel>> GetTopExpiringSubscriptionsAsync(int rowCount);

    }
}
