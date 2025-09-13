using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.Shared;
using InfinionInterviewProject.Infrastructure.Persistence;

namespace InfinionInterviewProject.Application.Interfaces 
{
    public interface IStateService
    {
        Task<ApiResponse<List<LgaSeed>>> GetLgasByStateAsync(string state);
    } 
}