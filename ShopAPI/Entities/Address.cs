namespace ShopAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
