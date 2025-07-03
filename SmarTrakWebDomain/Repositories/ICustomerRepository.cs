using SmarTrakWebDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Services
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerModel>> GetAllAsync(string? searchTerm, int page, int pageSize);

        Task<SPCustomerCountModel> GetCustomerCountAsync();
        Task<PagedResult<CustomerWithSubscriptionsModel>> GetCustomerSubscriptionsAsync(string? searchTerm, int page, int pageSize);
    }
}
