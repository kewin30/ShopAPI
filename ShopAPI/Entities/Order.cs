using System;
using System.Collections.Generic;

namespace ShopAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual List<Product> Products { get; set; }
        //public int? ProductId { get; set; }
        public DateTime? DateOfOrder { get; set; }
        public virtual OrderStatus Status { get; set; }
        public int StatusId { get; set; } = 4;
        public string OrderCode { get; set; }
    }
}
