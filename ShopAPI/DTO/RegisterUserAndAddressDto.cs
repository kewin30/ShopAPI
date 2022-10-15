using System;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class RegisterUserAndAddressDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Nationality { get; set; }
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //public int RoleId { get; set; } = 2;

        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }
    }
}
