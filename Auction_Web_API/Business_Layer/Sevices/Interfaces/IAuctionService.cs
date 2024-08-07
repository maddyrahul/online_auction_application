using Data_Access_Layer.DTOs;
namespace Business_Layer.Sevices.Interfaces
{
    public interface IAuctionService
    {
        List<AuctionDto> SearchAuctions(string searchValue);
        Task<IEnumerable<AuctionDto>> GetAllAuctions();
        Task<AuctionDto> GetAuctionById(int id);
        Task<AuctionDto> CreateAuction(AuctionDto auctionDto);
        Task<AuctionDto> UpdateAuction(int id, AuctionDto auctionDto);
        Task DeleteAuction(int id);

    }
}
