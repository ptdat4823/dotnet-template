using TMS.Application.DTOs.Order;

namespace TMS.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> SearchOrdersAsync(SearchOrderRequest req);
    }
}
