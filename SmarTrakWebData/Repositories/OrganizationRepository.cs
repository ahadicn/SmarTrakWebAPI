using Microsoft.EntityFrameworkCore;
using SmarTrakWebData.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AzureTestContext _context;

        public OrganizationRepository(AzureTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganizationModel>> GetAll()
        {
            return await _context.Organizations
                .Select(org => new OrganizationModel
                {
                    Id = org.Id,
                    Name = org.Name,
                    Address = org.Address
                })
                .ToListAsync();
        }

        public async Task<OrganizationModel> GetById(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            return org == null ? null : new OrganizationModel { Id = org.Id, Name = org.Name, Address = org.Address };
        }

        public async Task Add(OrganizationModel organization)
        {
            var org = new Organization { Name = organization.Name, Address = organization.Address };
            _context.Organizations.Add(org);
            await _context.SaveChangesAsync();
        }

        public async Task Update(OrganizationModel organization)
        {
            var org = await _context.Organizations.FindAsync(organization.Id);
            if (org != null)
            {
                org.Name = organization.Name;
                org.Address = organization.Address;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var org = await _context.Organizations.FindAsync(id);
            if (org != null)
            {
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
            }
        }
    }
}
