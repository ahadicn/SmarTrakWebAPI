using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly string _connectionString;
        public SubscriptionRepository(STContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<PagedResult<SubscriptionModel>> GetAllSubscriptionsAsync(GetAllSubscriptionResponseModel p)
        {
            var query = _context.Subscriptions.AsQueryable();

            // Text search
            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var term = p.SearchTerm.Trim().ToLower();
                query = query.Where(s =>
                    s.OfferName.ToLower().Contains(term) ||
                    s.Status.ToLower().Contains(term));
            }

            // Filters
            if (!string.IsNullOrWhiteSpace(p.Status)) query = query.Where(s => s.Status == p.Status);
            if (!string.IsNullOrWhiteSpace(p.UnitType)) query = query.Where(s => s.UnitType == p.UnitType);
            if (!string.IsNullOrWhiteSpace(p.BillingCycle)) query = query.Where(s => s.BillingCycle == p.BillingCycle);
            if (!string.IsNullOrWhiteSpace(p.BillingType)) query = query.Where(s => s.BillingType == p.BillingType);
            if (p.AutoRenewal.HasValue) query = query.Where(s => s.AutoRenewEnabled == p.AutoRenewal);
            if (!string.IsNullOrWhiteSpace(p.TermDuration)) query = query.Where(s => s.TermDuration == p.TermDuration);
            if (p.IsTrial.HasValue) query = query.Where(s => s.IsTrial == p.IsTrial);

            // Date Range Filters
            if (p.CreatedDateFrom.HasValue) query = query.Where(s => s.CreatedDate >= p.CreatedDateFrom.Value);
            if (p.CreatedDateTo.HasValue) query = query.Where(s => s.CreatedDate <= p.CreatedDateTo.Value);
            if (p.StartedDateFrom.HasValue) query = query.Where(s => s.StartedDate >= p.StartedDateFrom.Value);
            if (p.StartedDateTo.HasValue) query = query.Where(s => s.StartedDate <= p.StartedDateTo.Value);
            if (p.CommitmentEndDateFrom.HasValue) query = query.Where(s => s.CommitmentEndDate >= p.CommitmentEndDateFrom.Value);
            if (p.CommitmentEndDateTo.HasValue) query = query.Where(s => s.CommitmentEndDate <= p.CommitmentEndDateTo.Value);
            if (p.EffectiveStartDateFrom.HasValue) query = query.Where(s => s.EffectiveStartDate >= p.EffectiveStartDateFrom.Value);
            if (p.EffectiveStartDateTo.HasValue) query = query.Where(s => s.EffectiveStartDate <= p.EffectiveStartDateTo.Value);

            // Sorting
            var sortField = p.SortBy?.ToLower();
            bool descending = p.SortOrder?.ToLower() == "desc";

            query = sortField switch
            {
                "status" => descending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),
                "billingcycle" => descending ? query.OrderByDescending(s => s.BillingCycle) : query.OrderBy(s => s.BillingCycle),
                "billingtype" => descending ? query.OrderByDescending(s => s.BillingType) : query.OrderBy(s => s.BillingType),
                "unitType" => descending ? query.OrderByDescending(s => s.UnitType) : query.OrderBy(s => s.UnitType),
                "termDuration" => descending ? query.OrderByDescending(s => s.TermDuration) : query.OrderBy(s => s.TermDuration),
                "createddate" => descending ? query.OrderByDescending(s => s.CreatedDate) : query.OrderBy(s => s.CreatedDate),
                "starteddate" => descending ? query.OrderByDescending(s => s.StartedDate) : query.OrderBy(s => s.StartedDate),
                "commitmentenddate" => descending ? query.OrderByDescending(s => s.CommitmentEndDate) : query.OrderBy(s => s.CommitmentEndDate),
                "effectivestartdate" => descending ? query.OrderByDescending(s => s.EffectiveStartDate) : query.OrderBy(s => s.EffectiveStartDate),
                _ => query.OrderBy(s => s.OfferName)
            };

            // Pagination
            var total = await query.CountAsync();
            var items = await query
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .Select(s => new SubscriptionModel
                {
                    Id = s.Id,
                    SubscriptionId = s.SubscriptionId,
                    CustomerId = s.CustomerId,
                    OfferName = s.OfferName,
                    Status = s.Status,
                    Quantity = s.Quantity,
                    UnitType = s.UnitType,
                    BillingCycle = s.BillingCycle,
                    BillingType = s.BillingType,
                    CreatedDate = s.CreatedDate,
                    StartedDate = s.StartedDate,
                    CommitmentEndDate = s.CommitmentEndDate,
                    EffectiveStartDate = s.EffectiveStartDate,
                    AutoRenewEnabled = s.AutoRenewEnabled,
                    TermDuration = s.TermDuration,
                    IsTrial = s.IsTrial,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return new PagedResult<SubscriptionModel>
            {
                Items = items,
                TotalCount = total
            };
        }




        public async Task<IEnumerable<SubscriptionModel>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Subscriptions
                .Where(s => s.CustomerId == customerId)
                .Include(s => s.Customer)
                .Select(s => new SubscriptionModel
                {
                    Id = s.Id,
                    SubscriptionId = s.SubscriptionId,
                    CustomerId = s.CustomerId,
                    OfferName = s.OfferName,
                    Status = s.Status,
                    Quantity = s.Quantity,
                    UnitType = s.UnitType,
                    BillingCycle = s.BillingCycle,
                    BillingType = s.BillingType,
                    AutoRenewEnabled = s.AutoRenewEnabled,                    
                    EffectiveStartDate = s.EffectiveStartDate,
                    CommitmentEndDate = s.CommitmentEndDate,
                    TermDuration = s.TermDuration,
                    IsTrial = s.IsTrial,
                    CreatedDate = s.CreatedDate,
                    StartedDate = s.StartedDate,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    CreatedBy = s.CreatedBy,
                    UpdatedBy = s.UpdatedBy
                })
                .ToListAsync();
            //=> await _context.Subscriptions.Where(s => s.CustomerId == customerId).ToListAsync();
        }


        public async Task<SPSubscriptionCountModel> GetSubscriptionCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<SPSubscriptionCountModel>(
                    "sp_GetSubscriptionCount",
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }
        }


    }
}
