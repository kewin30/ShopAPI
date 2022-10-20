using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.DTO.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int StatusId { get; set; }
        public List<ProductsTest> Products { get; set; }
    }
}
