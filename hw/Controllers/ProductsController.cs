using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hw.Data;
using hw.Entities;
using hw.Dtos;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public ProductsController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Discount = productDto.Discount,
                Price = productDto.Price,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GetHigherPrice")]
        public async Task<ActionResult<IEnumerable<Product>>> GetHigherPrice()
        {
            var maxPrice = await _context.Products.MaxAsync(p => p.Price);

            return Ok(maxPrice);
        }

        [HttpGet("GetHigherDiscounts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetHigherDiscounts()
        {
            var maxDiscount = await _context.Products.MaxAsync(p => p.Discount);

            return Ok(maxDiscount);
        }
    }
}