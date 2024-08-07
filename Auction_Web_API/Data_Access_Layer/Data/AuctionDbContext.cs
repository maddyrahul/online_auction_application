using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Auctions)
                .WithOne(a => a.Seller)
                .HasForeignKey(a => a.SellerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete here

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bids)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Auction>()
                .HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascading delete here
        }
    }
}
