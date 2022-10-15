using System;
using System.Collections.Generic;

namespace ShopAPI.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime? DateOfOrder { get; set; }
        //Status
        public string Value { get; set; }
        //Products
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Category { get; set; }
        //public string Size { get; set; }
        //public int SomeOrderId { get; set; }
        //User
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        //Address
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        
    }
}
