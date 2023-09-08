using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureWorksWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AdventureWorks2019Context _context;

        public AddressController(AdventureWorks2019Context context)
        {
            this._context = context;
        }

        [HttpGet("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAddressByID(int addressId)
        {
            var address = _context.Addresses.Find(addressId);
            return address == null ? NotFound() : Ok(address);
        }


        [HttpGet("{businessEntityId}/{personId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAddressByBusinessEntityContactId(int businessEntityId, int personId)
        {
            Address address = null;
            List<BusinessEntityContact> businessEntityContacts = _context.BusinessEntityContacts.Where
                (f =>
                f.BusinessEntityId == businessEntityId &&
                f.PersonId == personId).ToList();
            if (businessEntityContacts != null && businessEntityContacts.Count > 0)
            {
                List<BusinessEntityAddress> businessEntityAddresses =
                                    _context.BusinessEntityAddresses.Where(
                                        f => f.BusinessEntityId == businessEntityId && f.AddressTypeId == 3).ToList();
                if (businessEntityAddresses != null && businessEntityAddresses.Count > 0)
                {
                    address = _context.Addresses.Find(businessEntityAddresses[0].AddressId);
                    if (address != null && address.StateProvinceId != 0)
                    {
                        address.StateProvince = (StateProvince)_context.StateProvinces.Find(address.StateProvinceId);
                    }
                }
            }

            return address == null ? NotFound() : Ok(address);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressType))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAddressTypes()
        {
            var addressTypes = await _context.AddressTypes.ToListAsync();
            return addressTypes == null ? NotFound() : Ok(addressTypes);
        }
    }
}