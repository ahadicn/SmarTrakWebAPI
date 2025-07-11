﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmarTrakWebAPI.DBEntities;
using SmarTrakWebDomain.EntryModels;
using SmarTrakWebDomain.Services;
using SmarTrakWebDomain.ViewModels;
using SmarTrakWebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SmarTrakWebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customer
        [HttpGet("ListCustomer")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] CustomerListEntryModel input)
        {
            try
            {
                var result = await _customerService.GetAllCustomersAsync(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to fetch customers", message = ex.Message });
            }
        }

        [HttpGet("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                    return NotFound(new { message = $"Customer with ID {id} not found." });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Failed to fetch customer details",
                    message = ex.Message
                });
            }
        }


       
        [HttpGet("GetCustomerWithSubscriptions")]
        public async Task<IActionResult> GetCustomerSubscriptions([FromQuery] string? searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _customerService.GetCustomerSubscriptionsAsync(searchTerm, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while fetching customer subscriptions", message = ex.Message });
            }
        }



        [HttpGet("CustomerSubscriptionCount/{customerId}")]
        public async Task<IActionResult> GetCustomerSubscriptionCount(Guid customerId)
        {
            try
            {
                var result = await _customerService.GetCustomerTabDataAsync(customerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to load customer subscription stats", message = ex.Message });
            }
        }


        //==========================================================================

        //private readonly STContext _context;

        //public CustomerController(STContext context)
        //{
        //    _context = context;
        //}


        //// GET: api/Customer
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        //{
        //    try
        //    {
        //        var customers = await _context.Customers
        //            .Select(c => new Customer
        //            {
        //                Id = c.Id,
        //                Name = c.Name,
        //                Domain = c.Domain
        //            })
        //            .ToListAsync();

        //        return Ok(customers);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "An error occurred while fetching customers", message = ex.Message });
        //    }
        //}

        //// GET: api/Customer/{customerId}/subscriptions
        //[HttpGet("{customerId}/subscriptions")]
        //public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptionsByCustomer(Guid customerId)
        //{
        //    try
        //    {
        //        var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
        //        if (!customerExists)
        //        {
        //            return NotFound(new { error = "Customer not found" });
        //        }

        //        var subscriptions = await _context.Subscriptions
        //            .Where(s => s.CustomerId == customerId)
        //            .Select(s => new Subscription
        //            {
        //                Id = s.Id,
        //                OfferName = s.OfferName,
        //                Status = s.Status,
        //                Quantity = s.Quantity,
        //                UnitType = s.UnitType,
        //                BillingCycle = s.BillingCycle,
        //                BillingType = s.BillingType,
        //                CreatedDate = s.CreatedDate,
        //                StartedDate = s.StartedDate,
        //                CreatedAt = s.CreatedAt,
        //                UpdatedAt = s.UpdatedAt                        
        //            })
        //            .ToListAsync();

        //        return Ok(subscriptions);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "An error occurred while fetching subscriptions", message = ex.Message });
        //    }
        //}
    }
}