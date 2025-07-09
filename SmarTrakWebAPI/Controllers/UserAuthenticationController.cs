using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmarTrakWebData.Repositories;
using SmarTrakWebDomain.Services;
using SmarTrakWebService;
using System.Security.Claims;

namespace SmarTrakWebAPI.Controllers
{
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userauthService;
        private readonly IUserService _userservice;
        public UserAuthenticationController(IUserAuthenticationService userauthService, IUserService userservice)
        {
            _userauthService = userauthService;
            _userservice = userservice;
        }


        [Authorize]
        [HttpGet("verify-authorized")]
        public IActionResult VerifyAuthorized()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            var userDetails = new
            {
                Oid = User.FindFirst("oid")?.Value ?? "N/A",
                Sub = User.FindFirst("sub")?.Value ?? "N/A",
                Name = User.FindFirst("name")?.Value ?? "N/A",
                Email = User.FindFirst("email")?.Value ?? User.FindFirst("preferred_username")?.Value ?? "N/A",
                Roles = User.FindAll("roles").Select(r => r.Value).ToList()
            };

            return Ok(userDetails);
        }


        [Authorize]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole()
        {
            var userId = User.FindFirst("oid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest("Missing user Object ID (oid claim)");

            try
            {
                await _userauthService.AssignRoleIfNotExistsAsync(userId);
                return Ok("Role assigned successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to assign role: {ex.Message}");
            }
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser()
        {
            try
            {
                await _userservice.SaveUserFromClaimsAsync(User);
                return Ok("User saved/updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving user: {ex.Message}");
            }
        }

    }
}
