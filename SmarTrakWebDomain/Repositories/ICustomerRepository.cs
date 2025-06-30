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
        Task<IEnumerable<CustomerModel>> GetAllAsync();
        Task<CustomerModel> GetByIdAsync(Guid id);
    }
}
