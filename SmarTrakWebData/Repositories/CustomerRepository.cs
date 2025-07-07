using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Services;
using SmarTrakWebDomain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly STContext _context;
        private readonly string _connectionString;


        public CustomerRepository(STContext context, IConfiguration configuration) 
        { 
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<PagedResult<CustomerModel>> GetAllAsync(string? searchTerm, int page, int pageSize)
        {
            var query = _context.Customers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                var isGuid = Guid.TryParse(searchTerm, out Guid guid);
                query = query.Where(c =>
                    (isGuid && c.CustomerId == guid) ||
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.Domain.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            // Apply pagination
            //query = query
            //    .OrderBy(c => c.Name) // sort for consistent paging
            //    .Skip((page - 1) * pageSize)
            //    .Take(pageSize);

            var items = await query
                .OrderBy(c => c.Name) // sort for consistent paging
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CustomerId = c.CustomerId,
                    Domain = c.Domain,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    CreatedBy = c.CreatedBy,
                    UpdatedBy = c.UpdatedBy
                })
                .ToListAsync();

            return new PagedResult<CustomerModel>
            {
                Items = items,
                TotalCount = totalCount
            };
            //=> await _context.Customers.ToListAsync();
        }


        public async Task<SPCustomerCountModel> GetCustomerCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await connection.QuerySingleAsync<SPCustomerCountModel>(
                    "sp_GetCustomerCount",
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }
        }


        public async Task<PagedResult<CustomerWithSubscriptionsModel>> GetCustomerSubscriptionsAsync(string? searchTerm, int page, int pageSize)
        {
            var query = _context.Customers
                .Include(c => c.Subscriptions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                var isGuid = Guid.TryParse(searchTerm, out Guid guid);

                query = query.Where(c =>
                    (isGuid && c.Id == guid) ||
                    c.Name.ToLower().Contains(searchTerm) ||
                    c.Domain.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerWithSubscriptionsModel
                {
                    Id = c.Id,
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Domain = c.Domain,
                    Subscriptions = c.Subscriptions.Select(s => new CustomerSubscriptionModel
                    {
                        Id = s.Id,
                        SubscriptionId = s.SubscriptionId,                        
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
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResult<CustomerWithSubscriptionsModel>
            {
                Items = customers,
                TotalCount = totalCount
            };
        }




    }
}
