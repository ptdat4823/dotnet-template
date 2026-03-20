using TMS.Application.DTOs.Order;
using TMS.Application.Interfaces.Services;

namespace TMS.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderService _orderService;

        public OrderService(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        public async Task<List<OrderDto>> SearchOrdersAsync(SearchOrderRequest req)
        {
            return await _orderService.SearchOrdersAsync(req);
        }
    }
}
