namespace TMS.Application.DTOs.Report
{
    public class LeaderBoardRequest
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public DateTime? FromDate { get; set; }
    }
}
