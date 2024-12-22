using AdventureWorksLT2022.Models;
using AdventureWorksLT2022.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksLT2022.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductController(ProductRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _repository.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // GET: api/Product/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _repository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Dados inválidos.");
                }

                await _repository.AddProductAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // PUT: api/Product/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid || id != product.ProductID)
                {
                    return BadRequest("Dados inválidos ou IDs não coincidem.");
                }

                var existingProduct = await _repository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                await _repository.UpdateProductAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _repository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado.");
                }

                await _repository.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
