using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebData.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly STContext _context;

        public UserRepository(STContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetByTenantUserIdAsync(Guid tenantUserId)
        {
            var user =  await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.TenantUserId == tenantUserId);

            if (user == null) return null;

            return new UserModel
            {
                Id = user.Id,
                TenantUserId = user.TenantUserId,
                Name = user.Name,
                Email = user.Email,
                Sub = user.Sub,
                RoleName = user.Role?.RoleName
            };
        }

        public async Task SaveUserFromClaimsAsync(ClaimsPrincipal user)
        {
            // Extract and validate tenant user ID (oid)
            var tenantUserIdStr = user.FindFirst("oid")?.Value;
            if (!Guid.TryParse(tenantUserIdStr, out Guid tenantUserId))
                throw new Exception("Invalid or missing 'oid' claim");

            // Resolve role name from claims (default to "User")
            var roleName = user.FindAll("roles").FirstOrDefault()?.Value ?? "User";

            // Find the role in DB
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
                throw new Exception($"Role '{roleName}' not found in database");

            // Map to domain model
            var userModel = new UserModel
            {
                TenantUserId = tenantUserId,
                Name = user.FindFirst("name")?.Value ?? "N/A",
                Email = user.FindFirst("email")?.Value ?? user.FindFirst("preferred_username")?.Value ?? "N/A",
                Sub = user.FindFirst("sub")?.Value ?? "N/A",
                RoleId = role.Id
            };

            // Save to DB
            await AddOrUpdateAsync(userModel);
        }


        public async Task AddOrUpdateAsync(UserModel userModel)
        {
            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.TenantUserId == userModel.TenantUserId);

            if (existing == null)
            {
                var userEntity = new User
                {
                    Id = Guid.NewGuid(),
                    TenantUserId = userModel.TenantUserId,
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Sub = userModel.Sub,
                    RoleId = userModel.RoleId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(userEntity);
            }
            else
            {
                existing.Name = userModel.Name;
                existing.Email = userModel.Email;
                existing.Sub = userModel.Sub;
                existing.RoleId = userModel.RoleId;
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

    }

}
