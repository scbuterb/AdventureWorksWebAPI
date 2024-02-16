using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureWorksWebAPI.Models;
using System.ComponentModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AdventureWorks2019Context _context;
        public AddressController(AdventureWorks2019Context context)
        {
            this._context = context;
        }

        /// <summary>
        /// Get an Address By Id Value
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpGet("{addressId}")]
        [EndpointDescription("GetAddressById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);
            if (address != null && address.StateProvinceId != 0)
            {
                address.StateProvince = await _context.StateProvinces.FindAsync(address.StateProvinceId).ConfigureAwait(true);
            }
            return address == null ? NotFound() : Ok(address);
        }

        /// <summary>
        /// Gets an address by budinessEntityId and personId
        /// </summary>
        /// <param name="businessEntityId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{businessEntityId}/{personId}")]
        [Description("GetAddressByBusinessEntityContactId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressByBusinessEntityContactId(int businessEntityId, int personId)
        {
            Address? address = null;
            BusinessEntityContact businessEntityContact =
                await _context.BusinessEntityContacts.Where
                (f => f.BusinessEntityId == businessEntityId &&
                f.PersonId == personId
                )
                .FirstOrDefaultAsync();

            if (businessEntityContact != null)
            {
                List<BusinessEntityAddress> businessEntityAddresses =
                    await _context.BusinessEntityAddresses.Where(
                                        f => f.BusinessEntityId == businessEntityId
                                        && f.AddressTypeId == 3
                                        ).ToListAsync();
                if (businessEntityAddresses != null && businessEntityAddresses.Count > 0)
                {
                    address = _context.Addresses.Find(businessEntityAddresses[0].AddressId);
                    if (address != null && address.StateProvinceId != 0)
                    {
                        address.StateProvince = _context.StateProvinces.FindAsync(address.StateProvinceId).GetAwaiter().GetResult();
                    }
                }
            }

            return address == null ? NotFound() : Ok(address);
        }

        /// <summary>
        /// Returns list of all Address Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("GetAddressTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressType))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressTypes()
        {
            List<AddressType> addressTypes = await _context.AddressTypes.ToListAsync();
            return addressTypes == null ? NotFound() : Ok(addressTypes);
        }

        [HttpGet]
        [Description("GetAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddresses()
        {
           
            if (_context.Addresses != null)
            {
                List<Address> addresses = await _context.Addresses
                .Include(a => a.StateProvince).Take(100).ToListAsync();
                return addresses == null ? NotFound() : Ok(addresses);
            }

            return NotFound();

        }
    }
}