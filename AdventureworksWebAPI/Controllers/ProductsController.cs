using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
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

        [HttpGet]
        public List<Product> GetProducts()
        {
           return _context.Products.ToList();
        }

        //[HttpPost]
        //public 
    }
}
