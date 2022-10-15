using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Entities;
using ShopAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productController;

        public ProductController(IProductService productController)
        {
            _productController = productController;
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Post([FromBody]ProductDto dto)
        {
            _productController.Create(dto);
            return Ok();
        }
        [HttpGet("{productId}")]
        public ActionResult<ProductDto> Get([FromRoute] int productId)
        {
            var product = _productController.GetById(productId);
            return Ok(product);
        }
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get()
        {
            var products = _productController.GetAll();
            return Ok(products);
        }
        [HttpPut("{productId}")]
        [Authorize(Roles ="Admin")]
        public ActionResult Update([FromRoute]int productId, [FromBody] ChangeQuantityProductDto dto)
        {
            _productController.ChangeQuantity(productId, dto);
            return NoContent();
        }
    }
}
