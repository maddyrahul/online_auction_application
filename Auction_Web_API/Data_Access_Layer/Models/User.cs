
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Data_Access_Layer.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsBanned { get; set; } = false;

        [JsonIgnore]
        public ICollection<Bid>? Bids { get; set; }

        [JsonIgnore]
        public ICollection<Auction>? Auctions { get; set; }
    }
}
