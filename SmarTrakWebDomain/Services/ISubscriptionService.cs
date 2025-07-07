using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Services
{
    public interface ISubscriptionService
    {
        Task<PagedResult<SubscriptionModel>> GetAllSubscriptionsAsync(GetAllSubscriptionEntryModel parameters);
        Task<IEnumerable<SubscriptionModel>> GetSubscriptionById(Guid Id);
        Task<SPSubscriptionCountModel> GetSubscriptionCountAsync();
        Task<List<SubscriptionCalendarViewModel>> GetSubscriptionCalendarAsync();
        
    }
}
