using SmarTrakWebDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Services
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationModel>> GetAll();
        Task<OrganizationModel> GetById(int id);
        Task Add(OrganizationModel org);
        Task Update(OrganizationModel org);
        Task Delete(int id);
    }
}
