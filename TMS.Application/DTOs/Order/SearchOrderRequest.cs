namespace TMS.Application.DTOs.Order
{
    public class SearchOrderRequest
    {
        public int? UserId { get; set; }

        public decimal? MinAmount { get; set; }

        public decimal? MaxAmount { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? SortBy { get; set; }

        public bool Desc { get; set; } = false;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
