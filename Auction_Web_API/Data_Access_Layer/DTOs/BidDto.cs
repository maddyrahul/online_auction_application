

namespace Data_Access_Layer.DTOs
{
    public class BidDto
    {
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }

    }
}
