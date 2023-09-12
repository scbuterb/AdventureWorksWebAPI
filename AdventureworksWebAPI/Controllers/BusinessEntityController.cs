using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessEntityController : ControllerBase
    {
        AdventureWorks2019Context context = new AdventureWorks2019Context();

        private readonly AdventureWorks2019Context _context;
        public BusinessEntityController(AdventureWorks2019Context context)
        {
            this._context = context;
        }

        [HttpGet("{businessEntityId}")]
        [EndpointDescription("GetBusinessEntityById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BusinessEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBusinessEntityById(int businessEntityId)
        {
            var businessEntity = await _context.BusinessEntityAddresses.FindAsync(businessEntityId);           
            return businessEntity == null ? NotFound() : Ok(businessEntity);
        }              
    }
}