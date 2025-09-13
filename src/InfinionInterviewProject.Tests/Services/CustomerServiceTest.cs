using Xunit;
using Moq;
using InfinionInterviewProject.Infrastructure.Services;
using InfinionInterviewProject.Application.DTOs.Request;
using InfinionInterviewProject.Domain.Entities;
using InfinionInterviewProject.Infrastructure.Repositories;
using InfinionInterviewProject.Infrastructure.Interfaces;
using InfinionInterviewProject.Infrastructure.Persistence;
using InfinionInterviewProject.Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using InfinionInterviewProject.Application.DTOs.Response.InfinionInterviewProject.Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using InfinionInterviewProject.API.Controllers;

namespace InfinionInterviewProject.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private readonly Mock<IEmailSmsService> _emailSmsService;
        private readonly Mock<AppDbContext> _dbContext;
        private readonly Mock<CustomerService> _customerService;
        private readonly Mock<JwtSettings> _jwtSettings;
        private Mock<IConfiguration> _configuration;
        private readonly CustomerController _controller;

        public CustomerServiceTests()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _emailSmsService = new Mock<IEmailSmsService>();
            _dbContext = new Mock<AppDbContext>();
            _jwtSettings = new Mock<JwtSettings>();
            _configuration = new Mock<IConfiguration>();
            _customerService = new Mock<CustomerService>(_dbContext.Object, _emailSmsService.Object, _customerRepoMock.Object, _jwtSettings.Object, _configuration.Object);
            _controller = new CustomerController(_customerService.Object);
        }

        [Fact]
        public async Task Onboard_Should_Return_Created_When_Successful()
        {
            // Arrange
            var request = new CustomerOnboardRequestDto
            {
                PhoneNumber = "08012345678",
                Email = "test@example.com",
                Password = "StrongP@ssword1",
                State = "lagos",
                Lga = "agege"
            };

            var response = new CustomerResponseDto
            {
                Id = Guid.NewGuid(),
                Email = request.Email
            };
            _customerService
                .Setup(s => s.OnboardAsync(request));

            // Act
            var result = await _controller.Onboard(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CustomerResponseDto>>(result);

            // Unwrap the result from ActionResult
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var responseValue = Assert.IsType<CustomerResponseDto>(createdResult.Value);

            Assert.Equal(request.Email, responseValue.Email);
        }

    }
}
