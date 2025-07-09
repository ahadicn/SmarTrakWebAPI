using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebDomain.Services
{
    public interface IUserService
    {
        Task SaveUserFromClaimsAsync(ClaimsPrincipal user);
    }

}
