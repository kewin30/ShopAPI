using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopAPI.DTO.Products;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Services
{
    public interface IProductService
    {
        GetProductsDto GetById(int productId);
        List<GetProductsDto> GetAll();
        void Create(CreateProductDto dto);
    }
    public class ProductService : IProductService
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(OrderDbContext dbContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _context = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public GetProductsDto GetById(int productId)
        {
            _logger.LogWarning($"Product with ID {productId} GetById action invoked");
            var product = _context.Products.FirstOrDefault(r => r.Id == productId);
            if(product is null || product.Id != productId)
            {
                _logger.LogError($"Product with ID {productId} not found");
                throw new NotFoundException("Product not found");
            }
            var productDto = _mapper.Map<GetProductsDto>(product);
            return productDto;
        }
        public List<GetProductsDto>GetAll()
        {
            var product = _context.Products.ToList();
            if (product is null)
                throw new NotFoundException("Products not found");
            var productDtos = _mapper.Map<List<GetProductsDto>>(product);
            return productDtos;
        }
        public void Create(CreateProductDto dto)
        {
            _logger.LogWarning($"Product CREATE action invoked");
            var productDto = _mapper.Map<Product>(dto);   
            _context.Products.Add(productDto);
            _context.SaveChanges();
        }
    }
}
