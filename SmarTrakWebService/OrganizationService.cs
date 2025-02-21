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
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _repository;

        public OrganizationService(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrganizationModel>> GetAll() => await _repository.GetAll();

        public async Task<OrganizationModel> GetById(int id) => await _repository.GetById(id);

        public async Task Add(OrganizationModel org) => await _repository.Add(org);

        public async Task Update(OrganizationModel org) => await _repository.Update(org);

        public async Task Delete(int id) => await _repository.Delete(id);
    }
}
