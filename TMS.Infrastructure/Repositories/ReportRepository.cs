using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TMS.Application.DTOs.Order;
using TMS.Application.DTOs.Report;
using TMS.Application.DTOs.Revenue;
using TMS.Application.Interfaces.Repositories;
using TMS.Domain.Entities;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories
{
    internal class ReportRepository : IReportRepository
    {
        protected readonly AppDBContext _context;
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
        private readonly IMemoryCache _cache;
        private readonly string ORDER_CACHE_KEY = "ORDER_KEY_abc";

        public ReportRepository(AppDBContext context) 
        {
            _context = context;
        }

        public async Task<List<OrderSummaryDto>> GetOrderSummary()
        {
            return await _context.OrderDb
                .GroupBy(o => o.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalOrders = g.Count(),
                    TotalAmount = g.Sum(o => o.Amount),
                    LastOrderDate = g.Max(o => o.CreatedAt)
                })
                .Join(
                    _context.User1Db,
                    o => o.UserId,
                    u => u.Id,
                    (o, u) => new OrderSummaryDto() 
                    { 
                        UserId = o.UserId,
                        UserName = u.Name,
                        TotalOrders = o.TotalOrders,
                        TotalAmount = o.TotalAmount,
                        LastOrderDate = o.LastOrderDate
                    }
                )
                .ToListAsync();
        }

        public async Task<List<LeaderBoardDto>> GetLeaderboard(LeaderBoardRequest req)
        {
            IQueryable<Order> query = _context.OrderDb;
            
            if (req.FromDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt > req.FromDate.Value);
            }

            var skip = (req.Page - 1) * req.PageSize;

            var groupData = await query
                .GroupBy(o => o.UserId)
                .Select(o => new {
                    UserId = o.Key,
                    TotalAmount = o.Sum(o => o.Amount)
                })
                .OrderByDescending(o => o.TotalAmount)
                .Join(
                    _context.User1Db, 
                    o => o.UserId, 
                    u => u.Id,
                    (o, u) => new
                    {
                        o.UserId,
                        UserName = u.Name,
                        o.TotalAmount,
                    })
                .Skip(skip)
                .Take(req.PageSize)
                .ToListAsync();

            decimal? prevAmount = null;
            int rank = 0;
            var res = new List<LeaderBoardDto>();

            for (int i = 0; i < groupData.Count; i++) {
                var data = groupData[i];

                if (data.TotalAmount != prevAmount)
                {
                    prevAmount = data.TotalAmount;
                    rank = i + 1;
                }

                res.Add(new LeaderBoardDto()
                {
                    Rank = rank,
                    UserId = data.UserId,
                    UserName = data.UserName,
                    TotalAmount = data.TotalAmount
                });
            }
            return res;
        }

        public async Task<List<RevenueDto>> GetRevenue(RevenueRequest req)
        {
            IQueryable<Order> query = _context.OrderDb;

            if (req.FromDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt > req.FromDate.Value);
            }

            if (req.ToDate.HasValue)
            {
                query = query.Where(o => o.CreatedAt < req.ToDate.Value);
            }

            if (req.GroupBy == "month")
            {
                return await query
                    .GroupBy(g => new { g.CreatedAt.Year, g.CreatedAt.Month })
                    .Select(g => new RevenueDto()
                    {
                        Date = g.Key.Year.ToString("yyyy") + "-" + g.Key.Month.ToString("MM"),
                        Revenue = g.Sum(o => o.Amount)
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync();
            }
            else if (req.GroupBy == "year")
            {
                return await query
                    .GroupBy(g => g.CreatedAt.Year)
                    .Select(g => new RevenueDto()
                    {
                        Date = g.Key.ToString("yyyy"),
                        Revenue = g.Sum(o => o.Amount)
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync();
            }

            return await query
                .GroupBy(g => g.CreatedAt.Date)
                .Select(g => new RevenueDto()
                {
                    Date = g.Key.ToString("yyyy-MM-DD"),
                    Revenue = g.Sum(o => o.Amount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            if (_cache.TryGetValue(ORDER_CACHE_KEY, out List<OrderDto>? orders)) 
            {
                return orders ?? [];
            }

            await _cacheLock.WaitAsync();
            try
            {
                if (_cache.TryGetValue(ORDER_CACHE_KEY, out orders))
                {
                    return orders ?? [];
                }

                orders = await _context.OrderDb
                    .Where(o => o.UserId == userId)
                    .Select(o => new OrderDto()
                    {
                        Id = o.Id,
                        UserId = userId,
                        Amount = o.Amount,
                        CreatedAt = o.CreatedAt,
                    })
                    .ToListAsync();
                var expiredTime = TimeSpan.FromMinutes(5);
                _cache.Set(ORDER_CACHE_KEY, orders, expiredTime);
            }
            catch (Exception ex) {
                throw new Exception($"Error when getting orders: {ex.Message}");
            }
            finally { 
                _cacheLock.Release(); 
            }

            return orders;
        }

        public async Task CreateOrder(CreateOrderDto req)
        {
            var newOrder = new Order()
            { 
                Id = Guid.NewGuid().Variant,
                UserId= req.UserId,
                Amount = req.Amount,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.OrderDb.AddAsync(newOrder);
            _cache.Remove(ORDER_CACHE_KEY);
        }
    }
}
