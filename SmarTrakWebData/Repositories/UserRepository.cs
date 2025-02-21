using Microsoft.EntityFrameworkCore;
using SmarTrakWebData.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AzureTestContext _context;

        public UserRepository(AzureTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(user => new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                OrganizationId = user.OrganizationId
            });
        }

        public async Task<UserModel> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                OrganizationId = user.OrganizationId
            };
        }

        public async Task Add(UserModel userModel)
        {
            var userEntity = new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                OrganizationId = userModel.OrganizationId
            };

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();
            userModel.Id = userEntity.Id; // Assign newly created Id back to model
        }

        public async Task Update(UserModel userModel)
        {
            var userEntity = await _context.Users.FindAsync(userModel.Id);
            if (userEntity == null) return;

            userEntity.FirstName = userModel.FirstName;
            userEntity.LastName = userModel.LastName;
            userEntity.Email = userModel.Email;
            userEntity.OrganizationId = userModel.OrganizationId;

            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
