using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.IRepositories
{
    public interface IBidRepository
    {
        Task<IEnumerable<Bid>> GetAllBids();
        Task<Bid> GetBidById(int id);
        Task<Bid> CreateBid(Bid bid);
        Task<Bid> UpdateBid(Bid bid);
        Task DeleteBid(int id);
    }
}
