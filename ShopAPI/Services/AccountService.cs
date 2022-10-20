using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopAPI.DTO.User;
using ShopAPI.DTO.User_Address;
using ShopAPI.Entities;
using ShopAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ShopAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserAndAddressDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly OrderDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly IMapper _mapper;
        public AccountService(OrderDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
            _mapper = mapper;
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users.Include(u=>u.Role).FirstOrDefault(u => u.Email == dto.Email);
            if(user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,$"{user.FirstName}{user.LastName}"),
                new Claim(ClaimTypes.Role,$"{user.Role.Name}"),
                //new Claim("dateOfBirth",user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
            };
            if (!string.IsNullOrEmpty(user.Nationality))
            {
                claims.Add(
                    new Claim("Nationality", user.Nationality)
                    );
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
              authenticationSettings.JwtIssuer,
              claims,
              expires: expires,
              signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserAndAddressDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                Nationality = dto.Nationality,
                PhoneNumber = dto.PhoneNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Address = new Address()
                {
                    City = dto.City,
                    Street = dto.Street,
                    ZipCode = dto.ZipCode,
                    BuildingNumber = dto.BuildingNumber,
                    FlatNumber = dto.FlatNumber
                }
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}
