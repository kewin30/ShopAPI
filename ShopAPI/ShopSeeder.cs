using ShopAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShopAPI
{
    public class ShopSeeder
    {
        private readonly OrderDbContext _dbContext;

        public ShopSeeder(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Products.Any())
                {
                    var products = GetProducts();
                    _dbContext.Products.AddRange(products);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Orders.Any())
                {
                    var order = GetOrders();
                    _dbContext.Orders.AddRange(order);
                    _dbContext.SaveChanges();
                }

            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name="User"
                },
                new Role()
                {
                    Name="Admin"
                }
            };
            return roles;
        }

        private IEnumerable<Order> GetOrders()
        {
            var orders = new List<Order>()
            {
                new Order()
                {
                    CreatedBy = new User()
                    {
                        Email = "Test@Test",
                        PhoneNumber=12345678,
                        FirstName="Janusz",
                        Address = new Address()
                        {
                            City = "Warszawa",
                            Street = "Pokątna",
                            ZipCode="00-000"
                        },
                    },
                    OrderCode="SomeCode",
                }
            };
            return orders;
        }

    private IEnumerable<Product> GetProducts()
        {
            var orders = new List<Product>()
            {
                new Product()
                {
                    Name = "Koszulka",
                    Description = "Description1",
                    Category = "Koszulkowa",
                    Photo="hhtps/link",
                    //Quantity=10,
                    Size = "M",
                    //OrderId=1
                },
                 new Product()
                {
                    Name = "Koszulka1",
                    Description = "Description2",
                    Category = "Koszulkowa1",
                    Photo="hhtps/link2",
                    Size="XL",
                    //OrderId=1
                },
                  new Product()
                {
                    Name = "Koszulka2",
                    Description = "Description3",
                    Category = "Koszulkowa2",
                    Photo="hhtps/link3",
                    Size="L",
                    //OrderId=1
                },
            };
            return orders;
        }
    }
}
