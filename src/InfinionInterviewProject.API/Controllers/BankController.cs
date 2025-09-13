using Microsoft.AspNetCore.Mvc;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace InfinionInterviewProject.API.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankController : ControllerBase
    { 
        private readonly IBankService _bankService;
        public BankController(IBankService bankService) 
        { 
            _bankService = bankService;
        }
        [HttpGet] 
        public async Task<IActionResult> GetBanks()
        { 
            var banks = await _bankService.GetBanksAsync();
            return Ok(banks);
        }
    }
}