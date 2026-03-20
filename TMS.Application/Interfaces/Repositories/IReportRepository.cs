using TMS.Application.DTOs.Order;
using TMS.Application.DTOs.Report;
using TMS.Application.DTOs.Revenue;

namespace TMS.Application.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<List<OrderSummaryDto>> GetOrderSummary();

        Task<List<LeaderBoardDto>> GetLeaderboard(LeaderBoardRequest request);

        Task<List<RevenueDto>> GetRevenue(RevenueRequest req);
    }
}
