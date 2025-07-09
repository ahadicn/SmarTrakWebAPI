using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using SmarTrakWebDomain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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


            // CustomerId filter via a separate param
            if (model.SearchCustomerId.HasValue && model.SearchCustomerId != Guid.Empty)
            {
                query = query.Where(s => s.CustomerId == model.SearchCustomerId.Value);
            }

            // Text search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                var term = model.SearchTerm.Trim().ToLower();
                var isGuid = Guid.TryParse(model.SearchTerm, out Guid guidTerm);

                query = query.Where(s =>
                    (isGuid && (s.SubscriptionId == guidTerm )) ||
                    s.OfferName.ToLower().Contains(term) ||
                    s.Status.ToLower().Contains(term) ||
                    s.Customer.Name.ToLower().Contains(term));
                    
            }

            // Filters
            if (!string.IsNullOrWhiteSpace(model.Status)) query = query.Where(s => s.Status == model.Status);            
            if (model.AutoRenewal.HasValue) query = query.Where(s => s.AutoRenewEnabled == model.AutoRenewal);
                       

            // Sorting
            var sortField = model.SortBy?.ToLower();
            bool descending = model.SortOrder?.ToLower() == "desc";

            query = sortField switch
            {
                "status" => descending ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status),                
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




        public async Task<IEnumerable<SubscriptionModel>> GetSubscriptionById(Guid Id)
        {
            return await _context.Subscriptions
                .Where(s => s.Id == Id)
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


        public async Task<List<SubscriptionCalendarViewModel>> GetSubscriptionCalendarAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<SubscriptionCalendarViewModel>(
                "GetSubscriptionCalendar",
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }

        public async Task<List<TopExpiringSubscriptionViewModel>> GetTopExpiringSubscriptionsAsync(int rowCount)
        {
            var today = DateTime.UtcNow;

            return await _context.Subscriptions
                .Where(s => s.CommitmentEndDate != null && s.CommitmentEndDate > today)
                .OrderBy(s => s.CommitmentEndDate)
                .Take(rowCount)
                .Select(s => new TopExpiringSubscriptionViewModel
                {
                    SubscriptionId = s.SubscriptionId,
                    OfferName = s.OfferName,
                    CommitmentEndDate = s.CommitmentEndDate ?? DateTime.MaxValue,
                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.Name,
                    Status = s.Status
                })
                .ToListAsync();
        }


    }
}
