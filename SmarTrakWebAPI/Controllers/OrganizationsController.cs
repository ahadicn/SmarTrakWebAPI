using Microsoft.AspNetCore.Mvc;
using SmarTrakWebDomain.Models;
using SmarTrakWebDomain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmarTrakWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<IEnumerable<OrganizationModel>> Get() => await _organizationService.GetAll();

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationModel>> Get(int id)
        {
            var org = await _organizationService.GetById(id);
            return org == null ? NotFound() : Ok(org);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrganizationModel org)
        {
            await _organizationService.Add(org);
            return CreatedAtAction(nameof(Get), new { id = org.Id }, org);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrganizationModel org)
        {
            if (id != org.Id) return BadRequest();
            await _organizationService.Update(org);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _organizationService.Delete(id);
            return NoContent();
        }
    }
}
