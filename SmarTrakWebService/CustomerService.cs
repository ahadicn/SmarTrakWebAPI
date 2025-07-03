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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepository = customerRepo;
        }

        public async Task<PagedResult<CustomerModel>> GetAllCustomersAsync(string? searchTerm, int page, int pageSize)
        {
            return await _customerRepository.GetAllAsync(searchTerm, page, pageSize);
        }

        public async Task<SPCustomerCountModel> GetCustomerCountAsync()
        {
            return await _customerRepository.GetCustomerCountAsync();
        }

        public async Task<PagedResult<CustomerWithSubscriptionsModel>> GetCustomerSubscriptionsAsync(string? searchTerm, int page, int pageSize)
        {
            return await _customerRepository.GetCustomerSubscriptionsAsync(searchTerm, page, pageSize);
        }


    }

}
