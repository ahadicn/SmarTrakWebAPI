using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Services;
using SmarTrakWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarTrakWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerSubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;


        public CustomerSubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("GetAllSubscriptions")]
        public async Task<IActionResult> GetAllSubscriptions([FromQuery] GetAllSubscriptionEntryModel parameters)
        {
            try
            {
                var result = await _subscriptionService.GetAllSubscriptionsAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch subscriptions", message = ex.Message });
            }
        }


        // GET: api/CustomerSubscriptions/{CustomerId}/
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSubscriptionsByCustomer(Guid Id)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionsByCustomerAsync(Id);
                if (result == null)
                    return NotFound(new { error = "Customer not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch subscriptions", message = ex.Message });
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