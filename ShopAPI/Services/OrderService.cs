using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
using ShopAPI.DTO.User;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ShopAPI.Services
{
    public interface IOrderService
    {
        OrderDto GetById(int id);
        ProductDto GetProducts(LoginDto dto);
        void Delete(int id);
        void MakeOrder(int id, MakeOrderDto order);
        void UpdateStatusId(int id, UpdateStatusDto dto);
    }
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        public OrderService(OrderDbContext context, IMapper mapper, ILogger<OrderService> logger, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Order with id {id} DELETE action INVOKED!!");
            var orders = _context
                .Orders
                .FirstOrDefault(r=>r.Id==id);
            if(orders is null || orders.Id != id)
            {
                _logger.LogError($"Order with id {id} not found");
                throw new NotFoundException("Order not found!");
            }
            _context.Orders.Remove(orders);
            _context.SaveChanges();
        }
        public string ReturnUserId(LoginDto dto)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == dto.Email);
            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            };
            string test="";
            foreach (var item in claims)
            {
                test = item.Value;
            }
            return test;
        }

        public ProductDto GetProducts(LoginDto dto)
        {
            ReturnUserId(dto);
            //var products = _context.Orders.Select(x => x.Products).ToList(); Working
            var products = _context.Orders.Include(x => x.Products).FirstOrDefault(x=>x.Id==1);

            //var queryString = products.ToQueryString();
            if(products is null)
            {
                throw new NotFoundException("Order not found!");
            }
            var productsDto = _mapper.Map<ProductDto>(products);
            return productsDto; // NIE ZNAJDUJE PRODUKTÓW
        }
        //public List<OrderDto> GetAllById(int id)
        //{
        //    var orders = _context
        //        .Orders
        //        .Include(x => x.CreatedBy)
        //        .Include(x => x.CreatedBy.Address)
        //        .Include(x => x.Status)
        //        .Include(x => x.Products)
        //        .FirstOrDefault(x=>x.Id==id);
        //    if(orders is null)
        //    {
        //        throw new NotFoundException("Order not found!");
        //    }
        //    var orderDtos = _mapper.Map<List<OrderDto>>(orders);
        //    return orderDtos;
        //}

        public OrderDto GetById(int id)
        {
            _logger.LogWarning($"Order with ID {id} GetById action invoked");

            var order = _context
                .Orders
                .Include(x=>x.CreatedBy)
                .Include(x => x.CreatedBy.Address)
                .Include(x=>x.Status)
                .Include(x=>x.Products)
                .FirstOrDefault(x => x.Id == id);
            if (order is null || order.Id != id)
            {
                _logger.LogError($"Order with id {id} not found!");
                throw new NotFoundException("Order not found!");
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }
        public void MakeOrder(int id, MakeOrderDto dto)
        {
            var product = _context
                .Products
                .FirstOrDefault(r => r.Id == id);
            if(product is null)
            {
                throw new NotFoundException("Product not found!");
            }
            var order = _context.Orders.FirstOrDefault(r => r.Id == dto.OrderId);
            if(order is null)
            {
                throw new NotFoundException("Order not found!");
            }
            product.OrderId = dto.OrderId;
            _context.SaveChanges();
        }

        public void UpdateStatusId(int id, UpdateStatusDto dto)
        {
            Order status = _context.Orders.FirstOrDefault(x => x.Id == id);
            if(status is null)
            {
                throw new NotFoundException("Order not found!");
            }
            if(dto.StatusId < 1 || dto.StatusId > 4)
            {
                throw new BadRequestException("Wrong Id");
            }
            status.StatusId = dto.StatusId;
            _context.SaveChanges();
        }
    }
}
