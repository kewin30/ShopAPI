using System;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO.Order
{
    public class UserOrderDto
    {
        public int CreatedById { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime DateOfOrder { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }

    }
}
