using Xunit;
using Moq;
using InfinionInterviewProject.API.Controllers;
using InfinionInterviewProject.Application.DTOs.Request;
using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Application.DTOs.Response.InfinionInterviewProject.Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Castle.Components.DictionaryAdapter.Xml;

namespace InfinionInterviewProject.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomerController(_customerServiceMock.Object);
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

            _customerServiceMock
                .Setup(s => s.OnboardAsync(request));

            // Act
            var result = await _controller.Onboard(request);

            // Assert
            var createdResult = Assert.IsType<IActionResult>(result);
            var responseValue = Assert.IsType<CustomerResponseDto>(result);
            Assert.Equal(request.Email, responseValue.Email);
        }

        [Fact]
        public async Task Onboard_Should_Return_Created_When_Failed()
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

            _customerServiceMock
                .Setup(s => s.OnboardAsync(request));

            // Act
            var result = await _controller.Onboard(request);

            // Assert
            var createdResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.True(createdResult.StatusCode.Value == 400);
        }
    }
}
