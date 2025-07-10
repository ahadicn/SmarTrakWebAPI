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
    public interface ICustomerService
    {
        Task<PagedResult<CustomerModel>> GetAllCustomersAsync(CustomerListEntryModel input);
        Task<CustomerModel?> GetCustomerByIdAsync(Guid id);
        Task<SPCustomerCountModel> GetCustomerCountAsync();
        Task<PagedResult<CustomerWithSubscriptionsModel>> GetCustomerSubscriptionsAsync(string? searchTerm, int page, int pageSize);
        Task<List<CustomerTabViewModel>> GetCustomerTabDataAsync(Guid customerId);
    }
}
