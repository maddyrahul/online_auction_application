
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Data_Access_Layer.Models
{
    public class Auction
    {
        [Key]
        public int AuctionId { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? ReservedPrice { get; set; }
        public int DurationTime { get; set; } // Duration in seconds
        public int SellerId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsEnded { get; set; } = false;
        public DateTime? EndTime { get; set; } // New property to track when auction ends

        [JsonIgnore]
        public User? Seller { get; set; }

        [JsonIgnore]
        public ICollection<Bid>? Bids { get; set; }
    }
}
