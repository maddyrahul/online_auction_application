using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.Data;
using Data_Access_Layer.DTOs;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidsController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidDto>>> GetBids()
        {
            var bids = await _bidService.GetAllBids();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidDto>> GetBid(int id)
        {
            var bid = await _bidService.GetBidById(id);
            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }

        [HttpPost]
        public async Task<ActionResult<BidDto>> CreateBid(BidDto bidDto)
        {
            try
            {
                var createdBid = await _bidService.CreateBid(bidDto);
                return CreatedAtAction(nameof(GetBid), new { id = createdBid.BidId }, createdBid);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBid(int id, BidDto bidDto)
        {
            if (id != bidDto.BidId)
            {
                return BadRequest();
            }

            try
            {
                await _bidService.UpdateBid(id, bidDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            try
            {
                await _bidService.DeleteBid(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
