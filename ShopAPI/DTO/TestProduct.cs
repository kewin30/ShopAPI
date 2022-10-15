using System;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class TestProduct
    {
        public int Id { get; set; }
        public DateTime? DateOfOrder { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        //public int Quantity { get; set; }
        //[MaxLength(2)]
        public string Size { get; set; }
    }
}
