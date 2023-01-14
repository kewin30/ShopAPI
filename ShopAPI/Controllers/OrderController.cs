using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
using ShopAPI.DTO.User;
using ShopAPI.Entities;
using ShopAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("{orderId}")]
        [Authorize(Roles = "User")]
        public ActionResult<OrderDto> Get([FromRoute] int orderId)
        {
            var order = _orderService.GetById(orderId);
            return Ok(order);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            _orderService.Delete(id);
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Create([FromBody] MakeOrderDto dto)
        {
            _orderService.CreateOrder(dto);
            return NoContent();
        }
        [HttpPost("NoLogin")]
        public ActionResult CreateWithoutLogin([FromBody] MakeOrderDto dto)
        {
            _orderService.CreateOrderWithoutLogin(dto);
            return NoContent();
        }
        [HttpPut("status/{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateStatus([FromRoute] int id,UpdateStatusDto dto)
        {
            _orderService.UpdateStatusId(id,dto);
            return NoContent();
        }
    }
}
