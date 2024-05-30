using System.ComponentModel.DataAnnotations;

namespace Part_2.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Age { get; set; }
        public int YearsOfService { get; set; }
        public string About { get; set; }

        public User User { get; set; }
    }
}
