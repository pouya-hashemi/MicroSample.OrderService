using MicroSample.OrderService.Dtos;
using MicroSample.OrderService.Entities;
using MicroSample.OrderService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroSample.OrderService.Controllers
{
    [Route("OrderService/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IAppDbContext _appDbContext;

        public OrderController(IAppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _appDbContext.Orders
                .Include(i => i.OrderItems)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _appDbContext.Orders
                .Include(i => i.OrderItems)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderCreateDto orderCreateDto)
        {
            if (orderCreateDto == null ||
                orderCreateDto.orderItemCreateDtos == null ||
                !orderCreateDto.orderItemCreateDtos.Any())
            {
                return BadRequest("No Item Selected");
            }
            var order = new Order()
            {
                CustomerId = orderCreateDto.CustomerId,
                OrderItems = orderCreateDto.orderItemCreateDtos.Select(s => new OrderItem()
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                }).ToList()
            };
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction("GetById", new { Id = order.Id }, order);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _appDbContext.Orders.FirstOrDefaultAsync(f => f.Id == id);

            if (order is null)
                return NotFound();

            _appDbContext.Orders.Remove(order);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

       
    }
}
