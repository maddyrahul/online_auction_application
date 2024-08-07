using Data_Access_Layer.DTOs;
using Data_Access_Layer.Models;


namespace Data_Access_Layer.IRepositories
{
    public interface IAuctionRepository
    {
        List<AuctionDto> SearchAuctions(string searchValue);
        Task<IEnumerable<Auction>> GetAllAuctions();
        Task<Auction> GetAuctionById(int id);
        Task<Auction> CreateAuction(Auction auction);
        Task<Auction> UpdateAuction(Auction auction);
        Task DeleteAuction(int id);
    }
}
