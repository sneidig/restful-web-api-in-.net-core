using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Models.DTO;
using AutoMapper;

namespace RoyalVilla_API.Controllers
{
    [ApiController]
    [Route("api/villas")]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<VillaController> _logger;

        public VillaController(ApplicationDbContext db, IMapper mapper, ILogger<VillaController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
        {
            var villas = await _db.Villa.ToListAsync();
            var dtoResponseVilla = _mapper.Map<List<VillaDTO>>(villas);
            var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(dtoResponseVilla, "Villas retrieved successfully");

            return Ok(response);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaDTO>>> GetVillaById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.BadRequest("Villa ID must be greater than 0"));
                }

                var villa = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} is not found"));
                }

                return Ok(ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villa), "Records retrieved successfully"));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving villa with ID {VillaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the villa.");
            }
        }


        [HttpPost()]
        public async Task<ActionResult<VillaCreateDTO>> CreateVilla(VillaCreateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("Villa data is required");
                }

                var duplicateVilla = await _db.Villa.FirstOrDefaultAsync(u =>
                                                    u.Name.ToLower() == villaDTO.Name.ToLower());

                if (duplicateVilla != null)
                {
                    return Conflict($"A villa with the name '{villaDTO.Name}' already exists");
                }

                Villa villa = _mapper.Map<Villa>(villaDTO);
                villa.CreatedDate = DateTime.UtcNow;

                await _db.Villa.AddAsync(villa);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetVillaById), new { id=villa.Id }, _mapper.Map<VillaCreateDTO>(villa));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating villa");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the villa.");
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaUpdateDTO>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest("Villa data is required");
                }

                if (id != villaDTO.Id)
                {
                    return BadRequest("Villa ID in URL does not match Villa ID in request body");
                }

                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} was not found");
                }

                var duplicateVilla = await _db.Villa.FirstOrDefaultAsync(u =>
                                                    u.Name.ToLower() == villaDTO.Name.ToLower()
                                                    && u.Id != id);

                if (duplicateVilla != null)
                {
                    return Conflict($"A villa with the name '{villaDTO.Name}' already exists");
                }

                _mapper.Map(villaDTO, existingVilla);
                existingVilla.UpdatedDate = DateTime.UtcNow;
               
                await _db.SaveChangesAsync();

                return Ok(villaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating villa with ID {VillaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the villa.");
            }
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            try
            {
            
                var existingVilla = await _db.Villa.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVilla == null)
                {
                    return NotFound($"Villa with ID {id} was not found");
                }

                _db.Villa.Remove(existingVilla);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting villa with ID {VillaId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the villa.");
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
