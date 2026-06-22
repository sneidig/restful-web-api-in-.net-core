using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; }

        [Range(0, 999.99)]
        public decimal Rate { get; set; }

        [Range(1, int.MaxValue)]
        public int Sqft { get; set; }

        [Range(1, int.MaxValue)]
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }

    }
}
