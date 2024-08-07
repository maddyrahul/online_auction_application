using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.DTOs;
using Data_Access_Layer.IRepositories;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Sevices
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionRepository _auctionRepository;

        public BidService(IBidRepository bidRepository, IAuctionRepository auctionRepository)
        {
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<BidDto>> GetAllBids()
        {
            var bids = await _bidRepository.GetAllBids();
            return bids.Select(MapToDto);
        }

        public async Task<BidDto> GetBidById(int id)
        {
            var bid = await _bidRepository.GetBidById(id);
            return bid != null ? MapToDto(bid) : null;
        }

        public async Task<BidDto> CreateBid(BidDto bidDto)
        {
            var auction = await _auctionRepository.GetAuctionById(bidDto.AuctionId);
            if (auction == null)
            {
                throw new KeyNotFoundException("Auction not found");
            }

            if (auction.IsEnded)
            {
                throw new InvalidOperationException("This auction has already ended");
            }

            var highestBid = auction.Bids.Max(b => (decimal?)b.Amount) ?? 0;

            if (bidDto.Amount <= highestBid)
            {
                throw new InvalidOperationException($"Bid amount must be higher than the current highest bid of {highestBid:C}");
            }

            if (bidDto.Amount < auction.StartingPrice)
            {
                throw new InvalidOperationException("Bid amount must be greater than or equal to the starting price");
            }

            if (bidDto.Amount > auction.ReservedPrice)
            {
                throw new InvalidOperationException("Bid amount must be less than or equal to the reserved price");
            }

            var bid = new Bid
            {
                Amount = bidDto.Amount,
                AuctionId = bidDto.AuctionId,
                UserId = bidDto.UserId
            };

            var createdBid = await _bidRepository.CreateBid(bid);

            if (bidDto.Amount == auction.ReservedPrice)
            {
                auction.IsEnded = true;
                await _auctionRepository.UpdateAuction(auction);
            }

            return MapToDto(createdBid);
        }

        public async Task<BidDto> UpdateBid(int id, BidDto bidDto)
        {
            var existingBid = await _bidRepository.GetBidById(id);
            if (existingBid == null)
            {
                throw new KeyNotFoundException($"Bid with id {id} not found.");
            }

            existingBid.Amount = bidDto.Amount;
            var updatedBid = await _bidRepository.UpdateBid(existingBid);
            return MapToDto(updatedBid);
        }

        public async Task DeleteBid(int id)
        {
            await _bidRepository.DeleteBid(id);
        }

        private BidDto MapToDto(Bid bid)
        {
            return new BidDto
            {
                BidId = bid.BidId,
                Amount = bid.Amount,
                AuctionId = bid.AuctionId,
                UserId = bid.UserId
            };
        }
    }
}
