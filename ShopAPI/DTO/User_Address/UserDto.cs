using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO.User_Address
{
    public class UserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
    }
}
