using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
