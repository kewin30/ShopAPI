using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.DTO.User;
using ShopAPI.DTO.User_Address;
using ShopAPI.Entities;
using ShopAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly OrderDbContext _context;
        public AccountController(IAccountService accountService, OrderDbContext context)
        {
            _accountService = accountService;
            _context = context;
        }
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody]RegisterUserAndAddressDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
        //[HttpGet("{id}")]
        //public ActionResult<GetUserAndAddress> Get([FromRoute]int id)
        //{
        //    var users = _accountService.GetById(id);
        //    return Ok(users);
        //}
    }
}
