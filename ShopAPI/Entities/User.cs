﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ShopAPI.Entities
{
    
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        
        public virtual Role Role { get; set; }
        public int RoleId { get; set; } = 2;
    }
}
