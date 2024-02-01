using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public static readonly List<Product> availableProducts = [];
        [HttpPost]
        public IActionResult AddProduct(string name)
        {
            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            availableProducts.Add(product);
            return Ok($"Товар {product.Name}[{product.Id}] успешно добавлен");
        }
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(Guid productId)
        {
            Product product = availableProducts.First(p => p.Id == productId);
            if (product == null)
            {
                return NotFound($"Товар {productId} не найден");
            }
            availableProducts.Remove(product);
            return Ok($"Товар {product.Name}[{productId}] успешно удален");
        }
        [HttpGet]
        public IActionResult GetAvailableProducts()
        {
            if (availableProducts.Count == 0)
            {
                return Ok("Нет продуктов, доступных для заказа");
            }
            return Ok(availableProducts);
        }
    }
}
