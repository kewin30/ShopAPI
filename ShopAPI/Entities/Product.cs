using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Photo { get; set; }
        public string Size { get; set; }
        public virtual Order Orders { get; set; }
        public int? OrderId { get; set; }
        public string ProductCode { get; set; }
    }
}
