using System.Threading.Tasks;

namespace InfinionInterviewProject.Infrastructure.Interfaces
{
    public interface IEmailSmsService
    {
        Task SendOtpAsync(string phoneNumber, string email, string otp);
    }
}
