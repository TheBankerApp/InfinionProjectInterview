using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.Shared;

namespace InfinionInterviewProject.Infrastructure.Interfaces
{
    public interface IBankService
    {
        Task<ApiResponse<BankResponse>> GetBanksAsync();
    }
}