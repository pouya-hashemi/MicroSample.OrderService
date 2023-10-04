using MicroSample.OrderService.Dtos;
using MicroSample.OrderService.Entities;
using MicroSample.OrderService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroSample.OrderService.Controllers
{
    [Route("OrderService/Order/{orderId}/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IAppDbContext _appDbContext;

        public OrderItemController(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int orderId)
        {
            return Ok(await _appDbContext.Orders
                .Include(i => i.OrderItems)
                .Where(w => w.Id == orderId)
                .Select(s => s.OrderItems)
                .FirstOrDefaultAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int orderId, int id)
        {
            var orderItem = await _appDbContext.Orders
                .Include(i => i.OrderItems)
                .Where(w => w.Id == orderId)
                .Select(s => s.OrderItems
                            .Where(w => w.Id == id)
                            .FirstOrDefault())
                .FirstOrDefaultAsync();

            if (orderItem == null)
                return NotFound();

            return Ok(orderItem);

        }
        [HttpPost]
        public async Task<IActionResult> Post(int orderId, OrderItemCreateDto orderItemCreateDto)
        {
            if (orderItemCreateDto == null)
            {
                return BadRequest("No Item Selected");
            }
            var order = _appDbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(f => f.Id == orderId);

            if (order is null)
                return NotFound();

            if (order.OrderItems is null)
                order.OrderItems = new List<OrderItem>();

            var orderItem = new OrderItem()
            {
                ProductId = orderItemCreateDto.ProductId,
                Quantity = orderItemCreateDto.Quantity,
            };
            order.OrderItems.Add(orderItem);

            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetById", new { orderId = orderId, id = orderItem.Id }, orderItem);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int orderId,int id)
        {
            var order = await _appDbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();

            if (order is null)
                return NotFound();

            var orderItem=order.OrderItems
                .FirstOrDefault(f => f.Id == id);

            if (orderItem is null)
                return NotFound();

            order.OrderItems.Remove(orderItem);

            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
