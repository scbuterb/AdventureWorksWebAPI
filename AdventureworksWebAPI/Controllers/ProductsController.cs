using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksWebAPI.Models;

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AdventureWorks2019Context _context;

        public ProductsController(AdventureWorks2019Context context)
        {
            this._context= context;
        }

        /// <summary>
        /// This gets a lot of products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]       
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProduct(int productId) {
            var product = _context.Products.Find(productId);
            return product == null ? NotFound() : Ok(product);
        }

        /// <summary>
        ///comment This is a comment.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]      
        public async Task<IActionResult> AddProduct([FromBody] Product product) 
        {           
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddProduct), new { id = product.ProductId }, product);
        }
    }
}
