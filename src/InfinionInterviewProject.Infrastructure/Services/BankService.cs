using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Application.Shared;
using InfinionInterviewProject.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
namespace InfinionInterviewProject.Infrastructure.Services
{
    public class BankService:IBankService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BankService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankService(HttpClient httpClient, ILogger<BankService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<BankResponse>> GetBanksAsync()
        {
            try
            {
                if (_httpContextAccessor.HttpContext?.User?.Identity?.Name is not { } getLoggedInUser)
                    return new ApiResponse<BankResponse>("User not authenticated.", false);
                var response = await _httpClient.GetAsync("GetAllBanks");

                response.EnsureSuccessStatusCode();
                var contentStream = await response.Content.ReadAsStreamAsync();
                var bankResponse = await JsonSerializer.DeserializeAsync<BankResponse>(contentStream);
                return new ApiResponse<BankResponse>(bankResponse, "Bank records retrieved successfully", true);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while calling GetBanks API. Message: {Message}", ex.Message);
                return new ApiResponse<BankResponse>(null,"Unable to reach GetBanks API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while calling GetBanks API");
                return new ApiResponse<BankResponse>(null,ex.Message);
            }
        }
    }

}