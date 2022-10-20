using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO.User
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
