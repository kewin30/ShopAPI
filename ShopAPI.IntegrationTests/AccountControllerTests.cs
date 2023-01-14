using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShopAPI.DTO.User;
using ShopAPI.Entities;
using ShopAPI.IntegrationTests.Helpers;
using ShopAPI.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ShopAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();
        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    ServiceDescriptor dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<OrderDbContext>));
                    services.Remove(dbContextOptions);
                    services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                    services.AddDbContext<OrderDbContext>(options => options.UseInMemoryDatabase("ShopDb"));
                });
            }).CreateClient();
        }
        [Fact]  
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            RegisterUserAndAddressDto user = new RegisterUserAndAddressDto()
            {
                Email = "Janusz@wp.pl",
                Password = "testowe",
                ConfirmPassword = "testowe"
            };
            HttpContent httpContent = user.ToJsonHttpContent();

            HttpResponseMessage response = await _client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("Janusz", "testowe")]
        [InlineData(null, null)]
        [InlineData("","testowe")]
        [InlineData(null,"testowe")]
        [InlineData("testowe",null)]
        [InlineData("","")]
        public async Task RegisterUser_ForInValidModel_ReturnsBadRequest(string param, string param2)
        {
            RegisterUserAndAddressDto user = new RegisterUserAndAddressDto()
            {
                Email = param,
                Password = null,
                ConfirmPassword = "testowe"
            };
            HttpContent httpContent = user.ToJsonHttpContent();

            HttpResponseMessage response = await _client.PostAsync("/api/account/register", httpContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            _accountServiceMock.Setup(x => x.GenerateJwt(It.IsAny<LoginDto>())).Returns("jwt");
            LoginDto login = new LoginDto()
            {
                Email = "Test@test.com",
                Password = "testowe"
            };
            HttpContent httpContent = login.ToJsonHttpContent();  
            HttpResponseMessage response =  await _client.PostAsync("api/account/login", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
        }
    }
}
