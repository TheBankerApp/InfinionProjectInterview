using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InfinionInterviewProject.Application.DTOs;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using InfinionInterviewProject.Application.ActionFilters;

namespace InfinionInterviewProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService) { _customerService = customerService; }

        [HttpPost("onboard")]

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Onboard([FromBody] CustomerOnboardRequestDto dto)
        {
            var (success, message, id) = await _customerService.OnboardAsync(dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message, customerId = id });
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var response = await _customerService.AuthenticateCustomerAsync(request);
            if (response == null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = await _customerService.GetAllAsync();
            return Ok(list);
        }
    }
}
