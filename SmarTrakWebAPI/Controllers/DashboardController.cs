using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmarTrakWebDomain.Services;

namespace SmarTrakWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ICustomerService _customerService;


        public DashboardController(ISubscriptionService subscriptionService, ICustomerService customerService)
        {
            _subscriptionService = subscriptionService;
            _customerService = customerService;
        }

        //
        [HttpGet("CustomerCount")]
        public async Task<IActionResult> GetCustomerCountAsync()
        {
            try
            {
                var metrics = await _customerService.GetCustomerCountAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Server error", Details = ex.Message });
            }
        }

        [HttpGet("SubscriptionCount")]
        public async Task<IActionResult> GetSubscriptionCountAsync()
        {
            try
            {
                var metrics = await _subscriptionService.GetSubscriptionCountAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Server error", Details = ex.Message });
            }
        }

        [HttpGet("SubscriptionCalendar")]
        public async Task<IActionResult> GetSubscriptionContractCalendar()
        {
            try
            {
                var data = await _subscriptionService.GetSubscriptionCalendarAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to load subscription contract calendar",
                    message = ex.Message
                });
            }
        }
    }
}
