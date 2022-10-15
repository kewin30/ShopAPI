using AutoMapper;
using ShopAPI.DTO;
using ShopAPI.Entities;

namespace ShopAPI
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<User, GetUserAndAddress>()
                .ForMember(x => x.City, x => x.MapFrom(s => s.Address.City))
                .ForMember(x => x.Street, x => x.MapFrom(s => s.Address.Street))
                .ForMember(x => x.ZipCode, x => x.MapFrom(s => s.Address.ZipCode))
                .ForMember(x => x.BuildingNumber, x => x.MapFrom(s => s.Address.BuildingNumber));
            CreateMap<Order, OrderDto>()
                .ForMember(x => x.Email, x => x.MapFrom(s => s.CreatedBy.Email))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(s => s.CreatedBy.PhoneNumber))
                .ForMember(x => x.FirstName, x => x.MapFrom(s => s.CreatedBy.FirstName))
                .ForMember(x => x.City, x => x.MapFrom(s => s.CreatedBy.Address.City))
                .ForMember(x => x.Street, x => x.MapFrom(s => s.CreatedBy.Address.Street))
                .ForMember(x => x.ZipCode, x => x.MapFrom(s => s.CreatedBy.Address.ZipCode))
                .ForMember(x => x.BuildingNumber, x => x.MapFrom(s => s.CreatedBy.Address.BuildingNumber))
                .ForMember(x => x.Value, x => x.MapFrom(s => s.Status.Value)
                );
            //CreateMap<Order, TestProduct>()
            //    .ForMember(x => x.Name, x => x.MapFrom(s => s.Products.Name))
            //    .ForMember(x => x.Description, x => x.MapFrom(s => s.Products.Description))
            //    .ForMember(x => x.Category, x => x.MapFrom(s => s.Products.Category))
            //    .ForMember(x => x.Size, x => x.MapFrom(s => s.Products.Size));
            CreateMap<Order, ProductDto>()
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Products.Name))
                .ForMember(x => x.Description, x => x.MapFrom(s => s.Products.Description))
                .ForMember(x => x.Category, x => x.MapFrom(s => s.Products.Category))
                .ForMember(x => x.Size, x => x.MapFrom(s => s.Products.Size));
        }
    }
}
