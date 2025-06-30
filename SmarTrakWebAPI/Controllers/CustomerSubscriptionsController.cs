using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarTrakWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerSubscriptionsController : ControllerBase
    {
        //private readonly STContext _context;

        private readonly ISubscriptionService _subscriptionService;


        public CustomerSubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET: api/Customer/{customerId}/subscriptions
        [HttpGet("{customerId}/subscriptions")]
        public async Task<IActionResult> GetSubscriptionsByCustomer(Guid customerId)
        {
            try
            {
                var result = await _subscriptionService.GetSubscriptionsByCustomerAsync(customerId);
                if (result == null)
                    return NotFound(new { error = "Customer not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch subscriptions", message = ex.Message });
            }
        }







        //====================================================================================
        //// GET: api/CustomerSubscriptions
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerSubscriptions()
        //{
        //    try
        //    {
        //        var customers = await _context.Customers
        //            .Include(c => c.Subscriptions)
        //            .Select(c => new Customer
        //            {
        //                Id = c.Id,
        //                Name = c.Name,
        //                Domain = c.Domain,
        //                Subscriptions = c.Subscriptions.Select(s => new Subscription
        //                {
        //                    Id = s.Id,
        //                    OfferName = s.OfferName,
        //                    Status = s.Status,
        //                    Quantity = s.Quantity,
        //                    UnitType = s.UnitType,
        //                    BillingCycle = s.BillingCycle,
        //                    BillingType = s.BillingType,
        //                    CreatedDate = s.CreatedDate,
        //                    StartedDate = s.StartedDate,
        //                    CreatedAt = s.CreatedAt,
        //                    UpdatedAt = s.UpdatedAt
        //                }).ToList()
        //            })
        //            .ToListAsync();

        //        return Ok(customers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "An error occurred while fetching customer subscriptions", message = ex.Message });
        //    }
        //}
    }
}