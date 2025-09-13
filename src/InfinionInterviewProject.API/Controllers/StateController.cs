using Microsoft.AspNetCore.Mvc;
using InfinionInterviewProject.Application.Interfaces; 
namespace InfinionInterviewProject.API.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    { 
        private readonly IStateService _stateService;
        public StateController(IStateService stateService)
        { 
            _stateService = stateService;
        }
        [HttpGet("{state}")]
        public async Task<IActionResult> GetLgas(string state)
        { 
            var lgas = await _stateService.GetLgasByStateAsync(state); 
            return Ok(lgas);
        }
    }
}