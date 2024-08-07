using Data_Access_Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Sevices.Interfaces
{
    public interface IBidService
    {
        Task<IEnumerable<BidDto>> GetAllBids();
        Task<BidDto> GetBidById(int id);
        Task<BidDto> CreateBid(BidDto bidDto);
        Task<BidDto> UpdateBid(int id, BidDto bidDto);
        Task DeleteBid(int id);
    }
}
