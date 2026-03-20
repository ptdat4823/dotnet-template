namespace TMS.Application.DTOs.Report
{
    public class LeaderBoardDto
    {
        public int Rank { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
    }
}
