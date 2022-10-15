using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopAPI.DTO;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Services
{
    public interface IProductService
    {
        ProductDto GetById(int productId);
        List<ProductDto> GetAll();
        void Create(ProductDto dto);
        void ChangeQuantity(int productId,ChangeQuantityProductDto dto);
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

        public ProductDto GetById(int productId)
        {
            _logger.LogWarning($"Product with ID {productId} GetById action invoked");
            var product = _context.Products.FirstOrDefault(r => r.Id == productId);
            if(product is null || product.Id != productId)
            {
                _logger.LogError($"Product with ID {productId} not found");
                throw new NotFoundException("Product not found");
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
        public List<ProductDto>GetAll()
        {
            var product = _context.Products.ToList();
            if (product is null)
                throw new NotFoundException("Products not found");
            var productDtos = _mapper.Map<List<ProductDto>>(product);
            return productDtos;
        }
        public void Create(ProductDto dto)
        {
            _logger.LogWarning($"Product CREATE action invoked");
            var productDto = _mapper.Map<Product>(dto);   
            _context.Products.Add(productDto);
            _context.SaveChanges();
        }
        public void ChangeQuantity(int productId, ChangeQuantityProductDto dto)
        {
            _logger.LogWarning($"Product UPDATE action invoked");
            var product = _context.Products.FirstOrDefault(r => r.Id == productId);
            if(product is null)
            {
                _logger.LogError($"Product with ID {productId} not found");
                throw new NotFoundException("Product not found");
            }
                
            //if(dto.Quantity > product.Quantity)
            //{
            //    _logger.LogError($"Given quantity is bigger than quantity in Database!");
            //    throw new BadRequestException("Too many values");
            //}
            //product.Quantity -= dto.Quantity;
            _context.SaveChanges();
        }
    }
}
