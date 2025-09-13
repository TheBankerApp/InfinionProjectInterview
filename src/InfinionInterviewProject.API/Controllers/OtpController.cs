using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using InfinionInterviewProject.Application.Interfaces;

namespace InfinionInterviewProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public OtpController(ICustomerService customerService) { _customerService = customerService; }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromQuery] Guid customerId, [FromQuery] string code)
        {
            var ok = await _customerService.VerifyOtpAsync(customerId, code);
            if (!ok) return BadRequest(new { message = "Invalid or expired OTP" });
            return Ok(new { message = "Phone verified" });
        }
    }
}
