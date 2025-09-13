using Xunit;
using Moq;
using InfinionInterviewProject.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace InfinionInterviewProject.Tests.Services
{
    public class StateServiceTests
    {
        private readonly Mock<ILogger<StateService>> _stateRepoMock;
        private readonly Mock<HttpClient> _httpClient;
        private readonly StateService _stateService;

        public StateServiceTests()
        {
            _stateRepoMock = new Mock<ILogger<StateService>>();
            _httpClient = new Mock<HttpClient>();
            _stateService = new StateService(_httpClient.Object,_stateRepoMock.Object);
        }

        [Fact]
        public async Task GetStates_Should_Return_All_States()
        {
            // Arrange
            var state = "lagos";

            // Act
            var result = await _stateService.GetLgasByStateAsync(state);

            // Assert
            Assert.NotNull(result);
        }
    }
}
