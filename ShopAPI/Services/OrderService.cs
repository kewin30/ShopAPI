using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Services
{
    public interface IOrderService
    {
        OrderDto GetById(int id);
       // List<OrderDto> GetAllById(int id);
        ProductDto GetProducts();
        void Delete(int id);
        void MakeOrder(int id, MakeOrderDto order);
        void UpdateStatusId(int id, UpdateStatusDto dto);
    }
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        public OrderService(OrderDbContext context, IMapper mapper, ILogger<OrderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
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

        public ProductDto GetProducts()
        {
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
            //var orderId =_context.Orders.FirstOrDefault(x => x.Id == id);
            
            //if(orderId.Id != dto.OrderId)
            //{
            //    throw new NotFoundException("Order not found!");
            //}
            var order = _context
                .Products
                .FirstOrDefault(r => r.Id == id);
            if(order is null)
            {
                throw new NotFoundException("Product not found!");
            }
            order.OrderId = 1;
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
