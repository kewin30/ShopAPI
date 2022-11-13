using AutoMapper;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
using ShopAPI.DTO.User_Address;
using ShopAPI.Entities;

namespace ShopAPI
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();


            CreateMap<Order, OrderDto>()
                .ForMember(x => x.Email, x => x.MapFrom(s => s.CreatedBy.Email))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(s => s.CreatedBy.PhoneNumber))
                .ForMember(x => x.FirstName, x => x.MapFrom(s => s.CreatedBy.FirstName))
                .ForMember(x => x.City, x => x.MapFrom(s => s.Address.City))
                .ForMember(x => x.Street, x => x.MapFrom(s => s.Address.Street))
                .ForMember(x => x.ZipCode, x => x.MapFrom(s => s.Address.ZipCode))
                .ForMember(x => x.BuildingNumber, x => x.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(x => x.Value, x => x.MapFrom(s => s.Status.Value));
            CreateMap<Order, MakeOrderDto>()
                .ForMember(x => x.City, x => x.MapFrom(s => s.Address.City))
                .ForMember(x => x.Street, x => x.MapFrom(s => s.Address.Street))
                .ForMember(x => x.ZipCode, x => x.MapFrom(s => s.Address.ZipCode))
                .ForMember(x => x.BuildingNumber, x => x.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(x => x.FlatNumber, x => x.MapFrom(s => s.Address.FlatNumber));
            CreateMap<Order, GetProductsDto>();
            CreateMap<MakeOrderDto, Order>();
            CreateMap<UserDto, User>();
            CreateMap<Product, GetProductsDto>();
            CreateMap<CreateProductDto , Product>();
            CreateMap<UserOrderDto , Order>();
            CreateMap<Product, GetProductsDto>();
            CreateMap<Order, ProductDto>();
        }
    }
}
