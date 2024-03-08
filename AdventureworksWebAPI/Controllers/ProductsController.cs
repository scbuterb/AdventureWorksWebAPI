using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdventureWorksWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace AdventureWorksWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
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
        //[HttpGet]
        //[Description("GetProducts")]
        //public List<Product> GetProducts()
        //{
        //    return _context.Products.ToList();
        //}

        [HttpGet("{productId}")]
        [Description("GetProduct")]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddProduct), new { id = product.ProductId }, product);
        }

        [HttpGet]
        [Description("GetProductList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductList))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProductList()
        {
            FormattableString sqlProcString = $"dbo.usp_GetProductList";
            
            var productList = _context.ProductList.FromSql<ProductList>(sqlProcString).ToList();

            return productList == null ? NotFound() : Ok( productList);
          
        }

        [HttpGet]
        [Description("GetProductItemByID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductList))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProductItemByID(int? id)
        {
            FormattableString sqlProcString = $"dbo.usp_GetProductByID {id}";

            var product = _context.Products.FromSql<Product>(sqlProcString).ToList();

            return product == null ? NotFound() : Ok(product);

        }

        [HttpGet]
        [Description("GetProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductCategory>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProductCategories()
        {
            try
            {
                var productCategories = _context.ProductCategories.ToList();
                return productCategories == null ? NotFound() : Ok(productCategories);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Description("GetProductSubCategoriesByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductSubcategory>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProductSubCategoriesByCategoryId(int? categoryId)
        {
            if (categoryId == null) { return NotFound(); }

            try
            {
                var productCategories = _context.ProductSubcategories.Where(s=> s.ProductCategoryId == categoryId.Value).ToList();
                return productCategories == null ? NotFound() : Ok(productCategories);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Description("GetProductsBySubCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProductsBySubCategory(int? subCategoryId)
        {
            if (subCategoryId == null) { return NotFound(); }
            try
            {
                var products = _context.Products
                    .Include(p => p.ProductProductPhotos)
                    .ThenInclude(q => q.ProductPhoto)
                    .Where(f => f.ProductSubcategoryId == subCategoryId.Value
                    && f.ListPrice > 0);
             
                return products == null ? NotFound() : Ok(products);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Description("GetProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProducts()
        {
           
            try
            {
                var products = _context.Products
                    .Include(p => p.ProductProductPhotos)
                    .ThenInclude(q => q.ProductPhoto)                   
                    .Where(f => f.ListPrice > 0)
                    .Take(100)
                    .ToList();

                return products == null ? NotFound() : Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
