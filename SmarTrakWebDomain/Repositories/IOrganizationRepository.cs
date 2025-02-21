using SmarTrakWebDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Repositories
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<OrganizationModel>> GetAll();
        Task<OrganizationModel> GetById(int id);
        Task Add(OrganizationModel organization);
        Task Update(OrganizationModel organization);
        Task Delete(int id);
    }
}
