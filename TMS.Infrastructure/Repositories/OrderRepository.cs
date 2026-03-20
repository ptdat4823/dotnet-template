using Microsoft.EntityFrameworkCore;
using TMS.Application.DTOs.Order;
using TMS.Application.Interfaces.Repositories;
using TMS.Domain.Entities;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly AppDBContext _context;

        public OrderRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> SearchAsync(SearchOrderRequest req)
        {
            IQueryable<Order> query = _context.OrderDb;

            if (req.UserId.HasValue)
            {
                query = query.Where(o => o.UserId == req.UserId.Value);
            }

            if (req.MinAmount.HasValue)
            { 
                query = query.Where(o => o.Amount >= req.MinAmount.Value);
            }

            if (req.MaxAmount.HasValue)
            {
                query = query.Where(o => o.Amount <= req.MaxAmount.Value);
            }

            if (req.FromDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt > req.FromDate.Value);
            }

            if (req.ToDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt < req.ToDate.Value);
            }

            if (!string.IsNullOrEmpty(req.SortBy))
            {
                if (string.Equals(req.SortBy, "amount", StringComparison.OrdinalIgnoreCase))
                {
                    query = req.Desc ? query.OrderByDescending(o => o.Amount) : query.OrderBy(o => o.Amount);
                }

                if (string.Equals(req.SortBy, "createAt", StringComparison.OrdinalIgnoreCase))
                {
                    query = req.Desc ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt);
                }
            }
            else
            {
                query = req.Desc ? query.OrderByDescending(o => o.CreatedAt) : query.OrderBy(o => o.CreatedAt);
            }

            var skip = (req.Page - 1) * req.PageSize;

            return await query
                            .Skip(skip)
                            .Take(req.PageSize)
                            .Select(o => new OrderDto() 
                            {
                                Id = o.Id,
                                UserId = o.UserId,
                                Amount = o.Amount,
                                CreatedAt = o.CreatedAt
                            }).ToListAsync();
        }
    }
}
