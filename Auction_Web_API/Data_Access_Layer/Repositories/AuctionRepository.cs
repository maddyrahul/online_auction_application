using Data_Access_Layer.DTOs;
using Data_Access_Layer.Data;
using Data_Access_Layer.IRepositories;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
namespace Data_Access_Layer.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Auction>> GetAllAuctions()
        {
            return await _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .ToListAsync();
        }

        public async Task<Auction> GetAuctionById(int id)
        {
            return await _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.AuctionId == id);
        }

        public async Task<Auction> CreateAuction(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
            return auction;
        }

        public async Task<Auction> UpdateAuction(Auction auction)
        {
            _context.Entry(auction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return auction;
        }

        public async Task DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }
        }
        public List<AuctionDto> SearchAuctions(string searchValue)
        {
            try
            {
                var query = _context.Auctions.AsQueryable();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(auction =>
                        auction.ProductName.Contains(searchValue) ||
                        auction.Category.Contains(searchValue)
                    );
                }
                return query.Select(a => new AuctionDto
                {
                    AuctionId = a.AuctionId,
                    ProductName = a.ProductName,
                    Description = a.Description,
                    Category = a.Category,
                    StartingPrice = a.StartingPrice,
                    ReservedPrice = a.ReservedPrice,
                    DurationTime = a.DurationTime,
                    SellerId = a.SellerId,
                    IsEnded = a.IsEnded,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
