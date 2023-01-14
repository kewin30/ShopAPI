using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Linq;
using ShopAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using ShopAPI.DTO.Order;
using ShopAPI.IntegrationTests.Helpers;
using ShopAPI.DTO.Products;
using System.Collections.Generic;
using System.Threading;

namespace ShopAPI.IntegrationTests
{
    public class OrderControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public OrderControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        ServiceDescriptor dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<OrderDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<OrderDbContext>(options => options.UseInMemoryDatabase("ShopDb"));
                    });
                });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async void CreateOrder_WithValidModel_ReturnsNoContent()
        {
            await CreateOrderTemplate("Koszulka1", "Koszulka1", HttpStatusCode.NoContent);
        }

        [Fact]
        public async void CreateOrder_WithInValidModel_ReturnsNotFound()
        {
            await CreateOrderTemplate("Koszuleczka", "Koszulka1", HttpStatusCode.NotFound);
        }

        [Fact]
        public async void CreateOrderWithoutLogin_WithValidModel_ReturnsNoContentStatus()
        {
            await CreatingNoLoginOrderTemplate("Koszulkowa10", "Koszulkowa10", HttpStatusCode.NoContent);
        }

        [Fact]
        public async void Delete_ForNonAdmin_ReturnsForbidden()
        {
            await DeleteTemplate(2, HttpStatusCode.Forbidden);
        }

        [Fact]
        public async void Delete_ForNonAdmin_ReturnsNoContent()
        {
            await DeleteTemplate(1, HttpStatusCode.NoContent);
        }
        
        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "")]
        public async void CreateOrder_WithInValidModel_ReturnsBadRequest(string param, string param2)
        {
            await CreateOrderTemplate(param, param2, HttpStatusCode.BadRequest);
        }
        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData(null, "")]
        public async void CreateOrderWithoutLogin_WithInValidModel_ReturnsBadRequest(string param, string param2)
        {
            await CreatingNoLoginOrderTemplate(param, param2, HttpStatusCode.BadRequest);
        }
        [Fact]
        public async void AHCreateOrderWithoutLogin_WithInValidProductCode_ReturnsNotFound()
        {
            await CreatingNoLoginOrderTemplate("Koszuleczka10", "Koszuleczkowa1", HttpStatusCode.NotFound);

        }


        private async Task CreateOrderTemplate(string productCode, string orderProductCode, HttpStatusCode status)
        {
            Product product = new Product()
            {
                ProductCode = productCode
            };
            User user = new User()
            {
                Email = "Januszex@wp.pl",
                PasswordHash = "AQAAAAEAACcQAAAAEPjPtMvj1+lmAo7m0RHCiyZHKSBBIcj+uDyrasdpw8jOOOzIbWFiNpPCnblcpPyd+g==",
            };
            Order order = new Order()
            {
                CreatedById = 1,
                OrderCode = "Test",
            };
            SeedUser(user);
            SeedOrder(order);
            SeedProduct(product);
            MakeOrderDto model = new MakeOrderDto()
            {
                FirstName = "Test",
                Email = "Januszex@wp.pl",
                Password = "Testowy",
                ProductCode = new List<GetProductsAndQuantityDto>()
                {
                    new GetProductsAndQuantityDto()
                    {
                        ProductCode=orderProductCode,
                        Quantity = 1
                    },
                },
                
            };
            
            HttpContent httpContent = model.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("/api/order", httpContent);

            response.StatusCode.Should().Be(status);
        }
        private async Task CreatingNoLoginOrderTemplate(string productCode, string orderProductCode, HttpStatusCode status)
        {
            Product product = new Product()
            {
                ProductCode = productCode
            };
            SeedProduct(product);
            MakeOrderDto model = new MakeOrderDto()
            {
                FirstName = "Test",
                Email = "Januszex@wp.pl",
                Password = "Test",
                ProductCode = new List<GetProductsAndQuantityDto>()
                {
                    new GetProductsAndQuantityDto()
                    {
                        ProductCode=orderProductCode,
                        Quantity = 1
                    },
                },
            };
            HttpContent httpContent = model.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("/api/order/NoLogin", httpContent);

            response.StatusCode.Should().Be(status);
        }
        private async Task DeleteTemplate(int id, HttpStatusCode StatusCode)
        {
            Order order = new Order()
            {
                CreatedById = id,
                OrderCode = "Test",
            };
            SeedOrder(order);

            var response = await _client.DeleteAsync("/api/order/" + order.Id);

            response.StatusCode.Should().Be(StatusCode);
        }
        private void SeedOrder(Order order)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            OrderDbContext _dbContext = scope.ServiceProvider.GetService<OrderDbContext>();
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }
        private void SeedProduct(Product order)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            OrderDbContext _dbContext = scope.ServiceProvider.GetService<OrderDbContext>();
            _dbContext.Products.Add(order);
            _dbContext.SaveChanges();
        }
        private void SeedUser(User user)
        {
            IServiceScopeFactory scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using IServiceScope scope = scopeFactory.CreateScope();
            OrderDbContext _dbContext = scope.ServiceProvider.GetService<OrderDbContext>();
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}
