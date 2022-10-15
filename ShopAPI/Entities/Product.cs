namespace ShopAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Photo { get; set; }
        //public int Quantity { get; set; }
        public string Size { get; set; }
        public int SomeOrderId { get; set; }
        public virtual Order Order { get; set; }
        public int? OrderId { get; set; }
    }
}
