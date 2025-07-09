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
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;


        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("ListSubscription")]
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


        // GET: api/Subscriptions/{CustomerId}/
        [HttpGet("GetSubscription/{Id}")]
        public async Task<IActionResult> GetSubscriptionById(Guid Id)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionById(Id);
                if (result == null)
                    return NotFound(new { error = "Customer not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch subscriptions", message = ex.Message });
            }
        }

        [HttpGet("TopSubscriptions")]
        public async Task<IActionResult> GetTopExpiringSubscriptions([FromQuery] int rowCount = 5)
        {
            try
            {
                var result = await _subscriptionService.GetTopExpiringSubscriptionsAsync(rowCount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to load top expiring subscriptions",
                    message = ex.Message
                });
            }
        }

    }
}