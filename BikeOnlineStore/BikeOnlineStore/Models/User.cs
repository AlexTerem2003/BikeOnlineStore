using System.ComponentModel.DataAnnotations;

namespace BikeOnlineStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Surname { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Patronymic { get; set; }

        [Required]
        public string? PhoneHumber { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public int Role { get; set; } = 1;
    }
}
