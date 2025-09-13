using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using InfinionInterviewProject.Infrastructure.Services;
using InfinionInterviewProject.Application.DTOs.Response;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace InfinionInterviewProject.Tests.Services
{
    public class BankServiceTests
    {
        private readonly Mock<ILogger<BankService>> _bankRepoMock;
        private readonly Mock<HttpClient> _httpClient;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly BankService _bankService;

        public BankServiceTests()
        {
            _bankRepoMock = new Mock<ILogger<BankService>>();
            _httpClient = new Mock<HttpClient>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _bankService = new BankService(_httpClient.Object, _bankRepoMock.Object, _httpContextAccessor.Object);
        }
        [Fact]
        public async Task GetBanks_Should_Return_Bank_List()
        {
            // Arrange
            var bankService = new BankService(_httpClient.Object, _bankRepoMock.Object, _httpContextAccessor.Object);

            // Act
            var result = await bankService.GetBanksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data.result);
            Assert.Equal("Wema Bank", result.Data.result.First().bankName);
        }
        [Fact]
        public async Task GetBanks_Should_Return_No_Bank()
        {
            // Arrange
            var bankService = new BankService(_httpClient.Object, _bankRepoMock.Object, _httpContextAccessor.Object);

            // Act
            var result = await bankService.GetBanksAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}
