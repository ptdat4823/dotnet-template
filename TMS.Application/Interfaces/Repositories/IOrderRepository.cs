using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Application.DTOs.Order;

namespace TMS.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> SearchAsync(SearchOrderRequest req);

        Task<List<OrderDto>> GetOrdersByUserIdAsync(string userId);
    }
}
