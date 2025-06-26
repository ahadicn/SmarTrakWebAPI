using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SmarTrakWebAPI.Controllers
{
    public class AuthenticationController : ControllerBase
    {


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






        //private readonly AzureTestContext _context;

        //public AuthenticationController( AzureTestContext context)
        //{  _context = context; }

        //[HttpGet("me")]
        //public IActionResult GetProfile()
        //{
        //    var email = User.FindFirst(ClaimTypes.Email)?.Value;

        //    var user = _context.Users
        //        .Include(u => u.Role)
        //        .FirstOrDefault(u => u.Email == email);

        //    if (user == null)
        //        return Unauthorized("User not found in local DB.");

        //    return Ok(new
        //    {
        //        user.Email,
        //        user.FirstName,
        //        user.LastName,
        //        Role = user.Role?.Name
        //    });
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet("admin-area")]
        //public IActionResult GetAdminStuff()
        //{
        //    return Ok("Welcome, Admin!");
        //}
    }
}
