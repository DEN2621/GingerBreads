namespace ShopAPI.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Product>? Products { get; set; }
    }
}
