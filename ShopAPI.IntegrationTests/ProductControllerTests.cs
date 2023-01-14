using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopAPI.DTO.Products;
using ShopAPI.Entities;
using ShopAPI.IntegrationTests.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShopAPI.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public ProductControllerTests(WebApplicationFactory<Startup> factory)
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
        [Theory]
        [InlineData("Koszulka", "koszulkowa")]
        [InlineData("Ko", "Test")]
        [InlineData("Spodnie", "SuperWielkie")]
        public async Task CreateProduct_WithValidModel_ReturnsOk(string param, string param1)
        {

            CreateProductDto product = new CreateProductDto()
            {
                Name = param,
                Description = param1
            };

            HttpContent httpContent = product.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("/api/product", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("", "SuperWielkie")]
        [InlineData(null, "SuperWielkie")]
        [InlineData("", null)]
        public async Task CreateProduct_WithInValidModel_ReturnsBadRequest(string param, string param1)
        {

            CreateProductDto product = new CreateProductDto()
            {
                Name = param,
                Description = param1
            };

            HttpContent httpContent = product.ToJsonHttpContent();
            HttpResponseMessage response = await _client.PostAsync("/api/product", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
