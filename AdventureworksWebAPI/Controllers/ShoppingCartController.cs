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
                                                            .ThenInclude(ph => ph.ProductProductPhotos)
                                                            .ThenInclude(s => s.ProductPhoto)
                                                        .Where(f => f.ShoppingCartId == shoppingCartID)
                                                        .ToList();

            return shoppingCartItems == null ? NotFound() : Ok(shoppingCartItems);
        }

        [HttpPost("ShoppingCartItem")]

        public IActionResult PostNewShoppingCartItem(string? shoppingCartID, int productID, int? quantity)
        {
            try
            {
                string cartId = string.Empty;
                if (string.IsNullOrWhiteSpace(shoppingCartID))
                {
                    cartId = CreateNewShoppingCart();
                }
                else
                {
                    cartId = shoppingCartID;
                }

                int itemQuantity = quantity ?? 1;

                ShoppingCartItem _item = new ShoppingCartItem()
                {
                    ShoppingCartId = cartId,                   
                    DateCreated = DateTime.Now,
                    Quantity = itemQuantity,
                    ProductId = productID
                };

                _context.ShoppingCartItems.Add(_item);
                _context.SaveChangesAsync();

                return Ok(cartId);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        private string CreateNewShoppingCart()
        {
            try
            {
                var newShoppingCartID = Guid.NewGuid().ToString();
                _context.ShoppingCarts.AddAsync(new ShoppingCart()
                {
                    ShoppingCartId = newShoppingCartID,
                    DateCreated = DateTime.Now
                });

                _context.SaveChanges();

                return newShoppingCartID;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
