using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
    }
}
