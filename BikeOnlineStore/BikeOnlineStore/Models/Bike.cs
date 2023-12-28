using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeOnlineStore.Models
{
    public class Bike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string? Title { get; set; }

        [Required]
        [StringLength(15)]
        public string? Type { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public byte WheelDiameter { get; set; }

        [Required]
        [StringLength(12)]
        public string? FrameMaterial { get; set; }

        [Required]
        [StringLength(12)]
        public string? BrakeType { get; set; }

        [Required]
        public byte NumberOfSpeeds { get; set; }

        [Required]
        [StringLength(18)]
        public string? Depreciation { get; set; }

        [Required]
        public byte Weight { get; set; }

        [Required]
        [StringLength(10)]
        public string? QuantityInStock { get; set; }

        public string? CoverImagePath { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? CoverPhoto { get; set; }
    }
}
