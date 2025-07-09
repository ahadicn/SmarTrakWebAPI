using SmarTrakWebDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetByTenantUserIdAsync(Guid tenantUserId);
        Task SaveUserFromClaimsAsync(ClaimsPrincipal user);
        Task AddOrUpdateAsync(UserModel user);
    }

}
