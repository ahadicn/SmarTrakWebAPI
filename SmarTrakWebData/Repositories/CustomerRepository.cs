using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly STContext _context;
        public CustomerRepository(STContext context) => _context = context;

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Domain = c.Domain
                })
                .ToListAsync();
            //=> await _context.Customers.ToListAsync();
        }




        public async Task<CustomerModel> GetByIdAsync(Guid id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null) return null;

            return new CustomerModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Domain = customer.Domain
            };
            //=>  await _context.Customers.FindAsync(id);
        }


    }
}
