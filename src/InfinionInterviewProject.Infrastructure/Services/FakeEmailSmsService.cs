using System;
using System.Threading.Tasks;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Infrastructure.Interfaces;

namespace InfinionInterviewProject.Infrastructure.Services
{
    public class FakeEmailSmsService : IEmailSmsService
    {
        public Task SendOtpAsync(string phoneNumber, string email, string otp)
        {
            Console.WriteLine($"[FAKE SERVICE] OTP={otp} Phone={phoneNumber} Email={email}");
            return Task.CompletedTask;
        }
    }
}
