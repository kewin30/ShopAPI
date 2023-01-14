using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
using ShopAPI.DTO.User;
using ShopAPI.DTO.User_Address;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ShopAPI.Services
{
    public interface IOrderService
    {
        OrderDto GetById(int id);
        void Delete(int id);
        void CreateOrder(MakeOrderDto order);
        void CreateOrderWithoutLogin(MakeOrderDto order);
        void UpdateStatusId(int id, UpdateStatusDto dto);
    }
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserContextService userContextService;
        public OrderService(OrderDbContext context, IMapper mapper, ILogger<OrderService> logger, IPasswordHasher<User> passwordHasher, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _passwordHasher = passwordHasher;
            this.authorizationService = authorizationService;
            this.userContextService = userContextService;
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
            var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, orders,
             new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _context.Orders.Remove(orders);
            _context.SaveChanges();
        }
        private string ReturnUserId(MakeOrderDto dto)
        {
            _logger.LogWarning("User ReturnUserId method invoked!");
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
            string userId="";
            foreach (var item in claims)
            {
                userId = item.Value;
            }
            return userId;
        }
        public OrderDto GetById(int id)
        {
            _logger.LogWarning($"Order with ID {id} GetById action invoked");

            var order = _context
                .Orders
                .Include(x=>x.CreatedBy)
                .Include(x => x.Address)
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
        public void CreateOrder(MakeOrderDto dto)
        {
            _logger.LogWarning("Order CREATE action invoked!");
            CheckIfProductCodeIsEmpty(dto);
            string userId = ReturnUserId(dto);

            UserOrderDto userOrder = setOrderDto(dto,userId);
            var orderDto = _mapper.Map<Order>(userOrder);

            _context.SaveChanges();
            _context.Orders.Add(orderDto);
            _context.SaveChanges();

            int latestOrderId = orderDto.Id;

            setProducts(dto, latestOrderId);

            Address address = setAddress(dto,latestOrderId);
            _context.Addresses.Add(address);

            //ProductCodes(dto);
            _context.SaveChanges();
        }
        private void setProducts(MakeOrderDto dto, int latestOrderId)
        {
            foreach (var item in dto.ProductCode)
            {
                var checkProducts = _context.Products
                    .FirstOrDefault(x => x.ProductCode == item.ProductCode);
                if (checkProducts is null)
                {
                    throw new NotFoundException("Product not found!");
                }
                var productsQuantity = _context.Products
                    .Where(x => x.ProductCode == item.ProductCode)
                    .Where(x => x.OrderId == null)
                    .Count();
                if (item.Quantity > productsQuantity)
                {
                    throw new NotFoundException("Too many products has been chosen");
                }
            }
            foreach (var item in dto.ProductCode)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    var products = _context.Products.Where(x => x.OrderId == null).FirstOrDefault(x=>x.ProductCode==item.ProductCode);
                    products.OrderId=latestOrderId;
                    _context.SaveChanges();
                }
            }
            
        }
        private UserOrderDto setOrderDto(MakeOrderDto dto, string userId)
        {
            UserOrderDto userOrder = new UserOrderDto()
            {
                CreatedById = Convert.ToInt32(userId),
                DateOfOrder = dto.DateOfOrder,
            };
            return userOrder;
        }
        private Address setAddress(MakeOrderDto dto, int latestOrderId)
        {
            Address address = new Address()
            {
                City = dto.City,
                Street = dto.Street,
                ZipCode = dto.ZipCode,
                BuildingNumber = dto.BuildingNumber,
                FlatNumber = dto.FlatNumber,
                OrderId = latestOrderId
            };
            return address;
        }
        private void ProductCodes(MakeOrderDto dto)
        {

            Regex re = new Regex(@"\d+");
            List<Match> matches = new List<Match>();
            //Matches - miejsce gdzie zaczyna się liczba w INDEX
            //Matches - value - wartość liczby
            List<string> products = new List<string>();
            foreach (var item in dto.ProductCode)
            {
                matches.Add(re.Match(item.ProductCode));
                for (int i = 0; i < item.ProductCode.Length; i++)
                {
                    string proba = "";
                    foreach (var item1 in matches)
                    {
                        if(i != Convert.ToInt32(item1.Value))
                        {
                            proba += item.ProductCode;
                        }
                        if (i == Convert.ToInt32(item1.Value))
                        {
                            for (int j = 0; j < item.Quantity; j++)
                            {
                                int test = Convert.ToInt32(item1.Value) + j;
                                string concat = proba + test;
                                products.Add(concat);
                            }
                        }
                    }      
                }
            }
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
        private void CheckIfProductCodeIsEmpty(MakeOrderDto order)
        {
            foreach (var item in order.ProductCode)
            {
                if (String.IsNullOrEmpty(item.ProductCode))
                {
                    throw new BadRequestException("ProductCode can't be empty!");
                }
            }
        }
        public void CreateOrderWithoutLogin(MakeOrderDto order)
        {
            _logger.LogWarning("Order CREATE_ORDER_WITHOUT_LOGIN action invoked!");
            CheckIfProductCodeIsEmpty(order);


            UserDto userDto = new UserDto()
            {
                Email = order.Email,
                Password = null,
                PhoneNumber = order.PhoneNumber,
                FirstName = order.FirstName
            };
            var userMapDto = _mapper.Map<User>(userDto);
            _context.Users.Add(userMapDto);
            _context.SaveChanges();

            UserOrderDto userOrder = new UserOrderDto()
            {
                CreatedById = userMapDto.Id,
                DateOfOrder = order.DateOfOrder,
            };
            var orderDto = _mapper.Map<Order>(userOrder);

            _context.Orders.Add(orderDto);
            _context.SaveChanges();

            int latestOrderId = orderDto.Id;

            setProducts(order, latestOrderId);

            Address address = setAddress(order, latestOrderId);
            _context.Addresses.Add(address);

            _context.SaveChanges();
        }
    }
}
