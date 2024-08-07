using Business_Layer.Sevices.Interfaces;
using Data_Access_Layer.Data;
using Data_Access_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IAuctionService _auctionService;

        public AuctionsController(AuctionDbContext context, IAuctionService auctionService)
        {
            _context = context;
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctions()
        {
            try
            {
                var auctions = await _auctionService.GetAllAuctions();
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuction(int id)
        {
            try
            {
                var auction = await _auctionService.GetAuctionById(id);
                if (auction == null)
                {
                    return NotFound();
                }
                return Ok(auction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(AuctionDto auctionDto)
        {
            try
            {
                var createdAuction = await _auctionService.CreateAuction(auctionDto);
                return CreatedAtAction(nameof(GetAuction), new { id = createdAuction.AuctionId }, createdAuction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuction(int id, AuctionDto auctionDto)
        {
            if (id != auctionDto.AuctionId)
            {
                return BadRequest("Auction ID mismatch");
            }

            try
            {
                await _auctionService.UpdateAuction(id, auctionDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                await _auctionService.DeleteAuction(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<AuctionDto>> SearchAuctions(string searchValue)
        {
            try
            {
                var auctions = _auctionService.SearchAuctions(searchValue);
                if (auctions == null || !auctions.Any())
                {
                    return NotFound();
                }
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
