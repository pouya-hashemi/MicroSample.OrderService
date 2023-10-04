namespace MicroSample.OrderService.Dtos
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public List<OrderItemCreateDto> orderItemCreateDtos { get; set; }
    }
}
