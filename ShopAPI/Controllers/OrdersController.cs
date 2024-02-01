using Microsoft.AspNetCore.Mvc;
using ShopAPI.Models;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> orders = [];
        [HttpPost]
        public IActionResult CreateOrder(Guid? userId, List<Product> products)
        {
            if (products is null)
            {
                return BadRequest("Пустой заказ");
            }
            userId ??= Guid.NewGuid();
            List<Product> unavailableProducts = [];
            products.ForEach(product =>
            {
                if (!ProductsController.availableProducts.Any(p => p.Id == product.Id && p.Name == product.Name))
                {
                    unavailableProducts.Add(product);
                }
            });
            if (unavailableProducts.Count > 0)
            {
                string result = "Следующие товары недоступны для заказа:";
                unavailableProducts.ForEach(product => result += $" {product.Name}[{product.Id}],");
                return BadRequest(result[..^1]);
            }
            Order order = new()
            {
                Id = Guid.NewGuid(),
                UserId = (Guid)userId,
                Products = products
            };
            orders.Add(order);
            return Ok($"Заказ успешно создан. Id пользователя: [{userId}]");
        }
        [HttpDelete("{orderId}")]
        public IActionResult CancelOrder(Guid orderId)
        {
            Order order = orders.First(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound($"Заказ {orderId} не найден");
            }
            orders.Remove(order);
            return Ok($"Заказ {orderId} для пользователя {order.UserId} успешно отменен");
        }
        [HttpGet("{userId}")]
        public IActionResult GetUserOrders(Guid userId)
        {
            if (!orders.Any(o => o.UserId == userId))
            {
                return BadRequest($"Не найдено заказов для пользователя {userId}");
            }
            return Ok(orders.Where(o => o.UserId == userId).ToList());
        }
    }
}
