using System.Net.Http;
using System.Threading.Tasks;

namespace InfinionInterviewProject.Infrastructure.Services
{
    public class ExternalStateClient
    {
        private readonly HttpClient _http;
        public ExternalStateClient(HttpClient http) { _http = http; }

        public Task<string> FetchStatesRawAsync() => _http.GetStringAsync("https://nga-states-lga.onrender.com/fetch");

        public Task<string> FetchLgasRawAsync(string state) => _http.GetStringAsync($"https://nga-states-lga.onrender.com/?state={state}");
    }
}
