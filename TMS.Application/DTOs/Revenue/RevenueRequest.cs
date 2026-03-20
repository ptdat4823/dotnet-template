namespace TMS.Application.DTOs.Revenue
{
    public class RevenueRequest
    {
        public string GroupBy { get; set; } = "day"; // day | month | year

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
