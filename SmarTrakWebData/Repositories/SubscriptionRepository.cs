using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly STContext _context;
        public SubscriptionRepository(STContext context) => _context = context;

        public async Task<IEnumerable<SubscriptionModel>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Subscriptions
                .Where(s => s.CustomerId == customerId)
                .Include(s => s.Customer)
                .Select(s => new SubscriptionModel
                {
                    Id = s.Id,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.Name,
                    OfferName = s.OfferName,
                    Status = s.Status,
                    Quantity = s.Quantity,
                    UnitType = s.UnitType,
                    BillingCycle = s.BillingCycle,
                    BillingType = s.BillingType,
                    CreatedDate = s.CreatedDate,
                    StartedDate = s.StartedDate,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();
            //=> await _context.Subscriptions.Where(s => s.CustomerId == customerId).ToListAsync();
        }
        
    }
}
