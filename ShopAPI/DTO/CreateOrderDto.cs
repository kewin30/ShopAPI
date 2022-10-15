namespace ShopAPI.DTO
{
    public class CreateOrderDto
    {
        public string Login { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public int BuildingNumber { get; set; }
        public int FlatNumber { get; set; }
        public string Country { get; set; }
        public string OrderList { get; set; }
    }
}
