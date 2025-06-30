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
        Task<IEnumerable<SubscriptionModel>> GetByCustomerIdAsync(Guid customerId);
    }
}
