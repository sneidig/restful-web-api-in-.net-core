using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla_API.Controllers
{
    [ApiController]
    [Route("api/villas")]
    public class VillaController : ControllerBase
    {

        // Normally you would never output a route parameter(s) without validation,
        //  especially before processing with business logic, these are just a basic examples

        [HttpGet]
        public string GetVillas([FromQuery] string? street = null)
        {
            // Query parameter for filtering - optional, defaults to null
            if (street != null)
                return $"Villas on street: {street}";
            return "Get all villas";
        }

        [HttpGet("{id:int}")]
        public string GetVillaById(int id)
        {
            // Int constraint - only matches numeric values
            return $"VillaId: {id}";
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

    }
}
