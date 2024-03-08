using Microsoft.AspNetCore.Mvc;
using AdventureWorksWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AdventureWorks2019Context _context;

        public ShoppingCartController(AdventureWorks2019Context context)
        {
            this._context = context;
        }

        [HttpGet()]
        [Description("GetShoppingCartItemsByID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetShoppingCartItemsByID(string shoppingCartID)
        {
            List<ShoppingCartItem> shoppingCartItems = _context.ShoppingCartItems
                                                        .Include(p => p.Product)
                                                        .Where(f => f.ShoppingCartId == shoppingCartID)
                                                        .ToList();

            return shoppingCartItems == null ? NotFound() : Ok(shoppingCartItems);
        }

         
    }
}
