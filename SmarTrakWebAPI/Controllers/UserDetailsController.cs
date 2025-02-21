using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmarTrakWebData.DBEntities;
using SmarTrakWebDomain.Services;
using System;

namespace SmarTrakWebAPI.Controllers
{
    public class UserDetailsController : ControllerBase
    {
        private readonly AzureTestContext _context;
        private readonly IAzureFunctionService _azureFunctionService;

        public UserDetailsController(AzureTestContext context, IAzureFunctionService azureFunctionService)
        {
            _context = context;
            _azureFunctionService = azureFunctionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.UserDetails.ToListAsync();
            return Ok(users);
        }

        [HttpGet("azure")] // Call Azure Function
        public async Task<IActionResult> GetAzureData()
        {
            var data = await _azureFunctionService.GetAzureFunctionData();
            return Ok(data);
        }
    }
}
