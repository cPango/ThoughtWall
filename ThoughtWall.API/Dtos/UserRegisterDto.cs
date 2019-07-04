using System.ComponentModel.DataAnnotations;

namespace ThoughtWall.API.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "Specify password between 4-12 characters")]
        public string Password { get; set; }
    }
}