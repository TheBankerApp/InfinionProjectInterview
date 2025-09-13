using InfinionInterviewProject.Application.DTOs;
using InfinionInterviewProject.Application.DTOs.Request;
using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.DTOs.Response.InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Domain.Entities;
namespace InfinionInterviewProject.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<(bool Success, string Message, Guid? CustomerId)> OnboardAsync(CustomerOnboardRequestDto dto);
        Task<bool> VerifyOtpAsync(Guid customerId, string code);
        Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
        Task<LoginResponse> AuthenticateCustomerAsync(LoginRequestDto request);
    }
}