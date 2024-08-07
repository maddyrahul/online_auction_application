using Data_Access_Layer.Data;
using Data_Access_Layer.IRepositories;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;

        public BidRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetAllBids()
        {
            return await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Bid> GetBidById(int id)
        {
            return await _context.Bids
                .Include(b => b.Auction)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BidId == id);
        }

        public async Task<Bid> CreateBid(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<Bid> UpdateBid(Bid bid)
        {
            _context.Entry(bid).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task DeleteBid(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
            }
        }
    }
}
