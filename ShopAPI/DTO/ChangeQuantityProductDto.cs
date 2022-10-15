using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO
{
    public class ChangeQuantityProductDto
    {
        [Required]
        public int Quantity { get; set; }
    }
}
