namespace MicroSample.OrderService.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
