using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using SmarTrakWebDomain.Services;
using SmarTrakWebDomain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebService
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepository = customerRepo;
        }

        public async Task<PagedResult<CustomerModel>> GetAllCustomersAsync(CustomerListEntryModel input)
        {
            return await _customerRepository.GetAllCustomersAsync(input);
        }
        public async Task<CustomerModel?> GetCustomerByIdAsync(Guid id)
        {
            return await _customerRepository.GetCustomerByIdAsync(id);
        }



        public async Task<SPCustomerCountModel> GetCustomerCountAsync()
        {
            return await _customerRepository.GetCustomerCountAsync();
        }

        public async Task<PagedResult<CustomerWithSubscriptionsModel>> GetCustomerSubscriptionsAsync(string? searchTerm, int page, int pageSize)
        {
            return await _customerRepository.GetCustomerSubscriptionsAsync(searchTerm, page, pageSize);
        }


        public async Task<List<CustomerTabViewModel>> GetCustomerTabDataAsync(Guid customerId)
        {
            return await _customerRepository.GetCustomerTabDataAsync(customerId);
        }

    }

}
