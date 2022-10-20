using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO.Order;
using ShopAPI.DTO.Products;
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
        public ActionResult<OrderDto> Get([FromRoute] int orderId)
        {
            var order = _orderService.GetById(orderId);
            return Ok(order);
        }
        [HttpGet]
        public ActionResult<ProductDto> GetAll()
        {
            var products = _orderService.GetProducts();
            return Ok(products);
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _orderService.Delete(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] MakeOrderDto dto)
        {
            _orderService.MakeOrder(id,dto);
            return NoContent();
        }
        [HttpPut("status/{id}")]
        public ActionResult UpdateStatus([FromRoute] int id,UpdateStatusDto dto)
        {
            _orderService.UpdateStatusId(id,dto);
            return NoContent();
        }
        //[HttpGet("user/{Id}")]
        //public ActionResult<IEnumerable<ProductDto>> GetById(int Id)
        //{
        //    var products = _orderService.GetAllById(Id);
        //    return Ok(products);
        //}


    }
}
