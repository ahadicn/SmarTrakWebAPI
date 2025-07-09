using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Repositories;
using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;            
        }

        public async Task SaveUserFromClaimsAsync(ClaimsPrincipal user)
        {
            await _userRepository.SaveUserFromClaimsAsync(user);
        }
    }

}
