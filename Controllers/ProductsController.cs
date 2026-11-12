using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;
using ProductsApi.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<CreateProductRequest> products = new();

        // GET all with paging
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var totalItems = products.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var data = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new
            {
                data,
                meta = new { page, pageSize, totalItems, totalPages }
            };

            return Ok(response);
        }

        // GET by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            var product = products.FirstOrDefault(p => products.IndexOf(p) + 1 == id);
            if (product == null) throw new NotFoundException($"Product {id} not found.");
            return Ok(product);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateProductRequest request)
        {
            if (products.Any(p => p.Sku == request.Sku))
                throw new ConflictException($"SKU {request.Sku} already exists.");

            products.Add(request);
            var id = products.Count;
            return CreatedAtAction(nameof(GetById), new { id }, request);
        }

        // PUT
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, UpdateProductRequest request)
        {
            var product = products.FirstOrDefault(p => products.IndexOf(p) + 1 == id);
            if (product == null) throw new NotFoundException($"Product {id} not found.");

            product.Name = request.Name;
            product.Price = request.Price;
            product.Sku = request.Sku;

            return Ok(product);
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => products.IndexOf(p) + 1 == id);
            if (product == null) throw new NotFoundException($"Product {id} not found.");

            products.Remove(product);
            return NoContent();
        }
    }
}

