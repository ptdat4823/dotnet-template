using TMS.Application.DTOs.Order;
using TMS.Application.DTOs.Report;
using TMS.Application.DTOs.Revenue;
using TMS.Application.Interfaces.Repositories;
using TMS.Application.Interfaces.Services;

namespace TMS.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository) 
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<OrderSummaryDto>> GetOrderSummary()
        {
            return await _reportRepository.GetOrderSummary();
        }

        public async Task<List<LeaderBoardDto>> GetLeaderboard(LeaderBoardRequest req)
        {
            return await _reportRepository.GetLeaderboard(req);
        }

        public async Task<List<RevenueDto>> GetRevenue(RevenueRequest req)
        {
            return await _reportRepository.GetRevenue(req);
        }
    }
}
