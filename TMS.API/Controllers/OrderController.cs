using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.Order;
using TMS.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order/search
        [HttpGet(Name = "search")]
        public async Task<ActionResult<List<OrderDto>>> SearchOrdersAsync([FromQuery] SearchOrderRequest req)
        {
            var res = await _orderService.SearchOrdersAsync(req);
            return Ok(res);
        }
    }
}
