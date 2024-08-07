
namespace Data_Access_Layer.DTOs
{
    public class AuctionDto
    {
        public int AuctionId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? ReservedPrice { get; set; }
        public int DurationTime { get; set; } // Duration in decimal hours
        public int SellerId { get; set; }
        public bool IsEnded { get; set; } = false;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } // New property to track when auction ends
    }
}
