using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebService
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepo)
        {
            _subscriptionRepository = subscriptionRepo;
        }

        public async Task<IEnumerable<SubscriptionModel>> GetSubscriptionsByCustomerAsync(Guid customerId)
        {
            return await _subscriptionRepository.GetByCustomerIdAsync(customerId);
        }
    }
}
