using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Application.Shared;
using InfinionInterviewProject.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
namespace InfinionInterviewProject.Infrastructure.Services 
{ 
    public class StateService : IStateService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StateService> _logger;
        public StateService(HttpClient httpClient,ILogger<StateService> logger)
        { 
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ApiResponse<List<LgaSeed>>> GetLgasByStateAsync(string state)
        {
            try
            {
                var url = $"https://nga-states-lga.onrender.com/?state={state}";
                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();
                var contentStream = await response.Content.ReadAsStreamAsync();
                var lgaResponse = await JsonSerializer.DeserializeAsync<List<LgaSeed>>(contentStream,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResponse<List<LgaSeed>>(lgaResponse, "Lga records retrieved successfully", true);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while calling GetBanks API. Message: {Message}", ex.Message);
                return new ApiResponse<List<LgaSeed>>(null, "Unable to reach GetBanks API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while calling GetBanks API");
                return new ApiResponse<List<LgaSeed>>(null, ex.Message);
            }
        }
    }
}