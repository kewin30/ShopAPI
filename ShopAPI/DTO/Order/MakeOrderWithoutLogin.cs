using ShopAPI.DTO.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO.Order
{
    public class MakeOrderWithoutLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PhoneNumber { get; set; }

        public DateTime DateOfOrder { get; set; }
        [Required]
        public List<GetProductsAndQuantityDto> ProductCode { get; set; }
        //Address
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }
    }
}
