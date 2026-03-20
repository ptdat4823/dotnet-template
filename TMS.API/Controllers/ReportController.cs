using Microsoft.AspNetCore.Mvc;
using TMS.Application.DTOs.Order;
using TMS.Application.DTOs.Report;
using TMS.Application.DTOs.Revenue;
using TMS.Application.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService orderService)
        {
            _reportService = orderService;
        }

        // GET: api/reports/order-summary
        [HttpGet(Name = "order-summary")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetOrderSummary()
        {
            var res = await _reportService.GetOrderSummary();
            return Ok(res);
        }

        // GET: api/reports/leaderboard
        [HttpGet(Name = "leaderboard")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetLeaderBoard([FromQuery] LeaderBoardRequest req)
        {
            var res = await _reportService.GetLeaderboard(req);
            return Ok(res);
        }

        // GET: api/reports/leaderboard
        [HttpGet(Name = "leaderboard")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetRevenue([FromQuery] RevenueRequest req)
        {
            var res = await _reportService.GetRevenue(req);
            return Ok(res);
        }
    }
}
