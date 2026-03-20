namespace TMS.Application.DTOs.Order
{
    public class OrderSummaryDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public int TotalOrders { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime LastOrderDate { get; set; }
    }
}
