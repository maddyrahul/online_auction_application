using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.DTOs;
using Data_Access_Layer.IRepositories;
using Data_Access_Layer.Models;


namespace Business_Layer.Sevices
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionService(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public List<AuctionDto> SearchAuctions(string searchValue)
        {
            return _auctionRepository.SearchAuctions(searchValue);
        }

        public async Task<IEnumerable<AuctionDto>> GetAllAuctions()
        {
            var auctions = await _auctionRepository.GetAllAuctions();
            return auctions.Select(MapToDto);
        }

        public async Task<AuctionDto> GetAuctionById(int id)
        {
            var auction = await _auctionRepository.GetAuctionById(id);
            return auction != null ? MapToDto(auction) : null;
        }

        public async Task<AuctionDto> CreateAuction(AuctionDto auctionDto)
        {
            var auction = MapToEntity(auctionDto);
            auction.StartTime = DateTime.UtcNow;
            auction.EndTime = auction.StartTime.AddSeconds(auction.DurationTime);
            auction.IsEnded = false;

            var createdAuction = await _auctionRepository.CreateAuction(auction);
            return MapToDto(createdAuction);
        }

        public async Task<AuctionDto> UpdateAuction(int id, AuctionDto auctionDto)
        {
            var existingAuction = await _auctionRepository.GetAuctionById(id);
            if (existingAuction == null)
            {
                throw new KeyNotFoundException($"Auction with id {id} not found.");
            }

            // Update properties
            existingAuction.ProductName = auctionDto.ProductName;
            existingAuction.Description = auctionDto.Description;
            existingAuction.Category = auctionDto.Category;
            existingAuction.StartingPrice = auctionDto.StartingPrice;
            existingAuction.ReservedPrice = auctionDto.ReservedPrice;
            existingAuction.DurationTime = auctionDto.DurationTime;
            existingAuction.EndTime = existingAuction.StartTime.AddHours(auctionDto.DurationTime);

            var updatedAuction = await _auctionRepository.UpdateAuction(existingAuction);
            return MapToDto(updatedAuction);
        }

        public async Task DeleteAuction(int id)
        {
            await _auctionRepository.DeleteAuction(id);
        }

        private AuctionDto MapToDto(Auction auction)
        {
            return new AuctionDto
            {
                AuctionId = auction.AuctionId,
                ProductName = auction.ProductName,
                Description = auction.Description,
                Category = auction.Category,
                StartingPrice = auction.StartingPrice,
                ReservedPrice = auction.ReservedPrice,
                DurationTime = auction.DurationTime,
                SellerId = auction.SellerId,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                IsEnded = auction.IsEnded
            };
        }

        private Auction MapToEntity(AuctionDto auctionDto)
        {
            return new Auction
            {
                AuctionId = auctionDto.AuctionId,
                ProductName = auctionDto.ProductName,
                Description = auctionDto.Description,
                Category = auctionDto.Category,
                StartingPrice = auctionDto.StartingPrice,
                ReservedPrice = auctionDto.ReservedPrice,
                DurationTime = auctionDto.DurationTime,
                SellerId = auctionDto.SellerId,
                StartTime = auctionDto.StartTime,
                EndTime = auctionDto.EndTime,
                IsEnded = auctionDto.IsEnded
            };
        }
    }
}
