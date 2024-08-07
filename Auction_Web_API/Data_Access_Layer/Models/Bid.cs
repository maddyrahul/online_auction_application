
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data_Access_Layer.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public int AuctionId { get; set; }

        [JsonIgnore]
        public Auction? Auction { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
