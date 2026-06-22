using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace RoyalVilla_API.Controllers
{
    [ApiController]
    [Route("api/villas")]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Normally you would never output a route parameter(s) without validation,
        //  especially before processing with business logic, these are just a basic examples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillasAsync()
        {

            return Ok(await _db.Villa.ToListAsync());

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Villa>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Villa ID must be greater than 0");
                }

                var villa = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} is not found");
                }

                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"An error occurred while retrieving villa with ID {id}: {ex.Message}");
            }
        }


        [HttpPost()]
        public async Task<ActionResult<Villa>> CreateVilla(Villa villa)
        {
            try
            {
                if (villa == null)
                {
                    return BadRequest("Villa data is required");
                }

                await _db.Villa.AddAsync(villa);
                await _db.SaveChangesAsync();

                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while creating the villa: {ex.Message}");
            }
        }


        /*
        [HttpGet]
        public string GetVillas([FromQuery] string? street = null)
        {
            // Query parameter for filtering - optional, defaults to null
            if (street != null)
                return $"Villas on street: {street}";

            return "Get all villas";
        }



        [HttpGet("{name}")]
        public string GetVillaByName(string name)
        {
            // String parameters are implicit, no constraint needed
            return $"Villa by name: {name}";
        }

        [HttpGet("{id:int}/{name}")]
        public string GetVillaByIdAndName(int id, string name)
        {
            // Multiple parameters: typed followed by string
            return $"VillaId: {id}, Name: {name}";
        }

        [HttpGet("{minRating:decimal}")]
        public string GetVillasByMinRating(decimal minRating)
        {
            // Decimal constraint example
            return $"Villas with minimum rating: {minRating}";
        }
        */

    }
}
