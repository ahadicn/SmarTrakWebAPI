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

        public async Task<PagedResult<SubscriptionModel>> GetAllSubscriptionsAsync(GetAllSubscriptionEntryModel model)
        {
            var query = _context.Subscriptions.AsQueryable();

            // Text search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                var term = model.SearchTerm.Trim().ToLower();
                query = query.Where(s =>
                    s.OfferName.ToLower().Contains(term) ||
                    s.Status.ToLower().Contains(term));
            }

            // Filters
            if (!string.IsNullOrWhiteSpace(model.Status)) query = query.Where(s => s.Status == model.Status);
            if (!string.IsNullOrWhiteSpace(model.UnitType)) query = query.Where(s => s.UnitType == model.UnitType);
            if (!string.IsNullOrWhiteSpace(model.BillingCycle)) query = query.Where(s => s.BillingCycle == model.BillingCycle);
            if (!string.IsNullOrWhiteSpace(model.BillingType)) query = query.Where(s => s.BillingType == model.BillingType);
            if (model.AutoRenewal.HasValue) query = query.Where(s => s.AutoRenewEnabled == model.AutoRenewal);
            if (!string.IsNullOrWhiteSpace(model.TermDuration)) query = query.Where(s => s.TermDuration == model.TermDuration);
            if (model.IsTrial.HasValue) query = query.Where(s => s.IsTrial == model.IsTrial);

            // Date Range Filters
            if (model.CreatedDateFrom.HasValue) query = query.Where(s => s.CreatedDate >= model.CreatedDateFrom.Value);
            if (model.CreatedDateTo.HasValue) query = query.Where(s => s.CreatedDate <= model.CreatedDateTo.Value);
            if (model.StartedDateFrom.HasValue) query = query.Where(s => s.StartedDate >= model.StartedDateFrom.Value);
            if (model.StartedDateTo.HasValue) query = query.Where(s => s.StartedDate <= model.StartedDateTo.Value);
            if (model.CommitmentEndDateFrom.HasValue) query = query.Where(s => s.CommitmentEndDate >= model.CommitmentEndDateFrom.Value);
            if (model.CommitmentEndDateTo.HasValue) query = query.Where(s => s.CommitmentEndDate <= model.CommitmentEndDateTo.Value);
            if (model.EffectiveStartDateFrom.HasValue) query = query.Where(s => s.EffectiveStartDate >= model.EffectiveStartDateFrom.Value);
            if (model.EffectiveStartDateTo.HasValue) query = query.Where(s => s.EffectiveStartDate <= model.EffectiveStartDateTo.Value);

            // Sorting
            var sortField = model.SortBy?.ToLower();
            bool descending = model.SortOrder?.ToLower() == "desc";

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
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(s => new SubscriptionModel
                {
                    Id = s.Id,
                    SubscriptionId = s.SubscriptionId,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.Name,
                    CustomerRefId = s.CustomerRefId,
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




        public async Task<IEnumerable<SubscriptionModel>> GetByCustomerIdAsync(Guid Id)
        {
            return await _context.Subscriptions
                .Where(s => s.CustomerId == Id)
                .Include(s => s.Customer)
                .Select(s => new SubscriptionModel
                {
                    Id = s.Id,
                    SubscriptionId = s.SubscriptionId,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.Name,
                    CustomerRefId = s.CustomerRefId,
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
