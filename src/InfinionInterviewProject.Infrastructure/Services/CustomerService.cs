using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfinionInterviewProject.Application.DTOs;
using InfinionInterviewProject.Application.Interfaces;
using InfinionInterviewProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using InfinionInterviewProject.Application.DTOs.Request;
using InfinionInterviewProject.Application.DTOs.Response.InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Domain.Entities.InfinionInterviewProject.Domain.Entities;
using InfinionInterviewProject.Infrastructure.Interfaces;
using InfinionInterviewProject.Infrastructure.Persistence;
using InfinionInterviewProject.Application.DTOs.Response;
using InfinionInterviewProject.Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace InfinionInterviewProject.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _db;
        private readonly IEmailSmsService _mailer;
        private readonly JwtSettings _jwtSettings;
        private readonly ICustomerRepository _customerRepo;
        private IConfiguration _configuration;

        public CustomerService(AppDbContext db, IEmailSmsService mailer, ICustomerRepository customerRepo, JwtSettings jwtSettings, IConfiguration configuration)
        {
            _db = db;
            _mailer = mailer;
            _customerRepo = customerRepo;
            _jwtSettings = jwtSettings;
            _configuration = configuration;
        }

        public async Task<(bool Success, string Message, Guid? CustomerId)> OnboardAsync(CustomerOnboardRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber)) return (false, "Phone is required", null);
            if (string.IsNullOrWhiteSpace(dto.Email)) return (false, "Email is required", null);
            if (string.IsNullOrWhiteSpace(dto.Password)) return (false, "Password is required", null);

            var lga = await _db.Lgas.FirstOrDefaultAsync(x => x.Name == dto.Lga && x.State == dto.State);
            if (lga == null) return (false, "LGA does not belong to State or not found", null);

            var exists = await _db.Customers.AnyAsync(c => c.PhoneNumber == dto.PhoneNumber || c.Email == dto.Email);
            if (exists) return (false, "Customer already exists", null);

            var cust = new Customer
            {
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                State = dto.State,
                Lga = dto.Lga,
                IsPhoneVerified = false
            };

            _db.Customers.Add(cust);

            var otp = new OtpCode
            {
                CustomerId = cust.Id,
                Code = GenerateOtp(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };
            _db.Otps.Add(otp);

            await _db.SaveChangesAsync();

            await _mailer.SendOtpAsync(cust.PhoneNumber, cust.Email, otp.Code);

            return (true, "OTP sent", cust.Id);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            return await _db.Customers.Select(c => new CustomerResponseDto
            {
                Id = c.Id,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                State = c.State,
                Lga = c.Lga,
                IsPhoneVerified = c.IsPhoneVerified,
                CreatedAt = c.CreatedAt
            }).ToListAsync();
        }

        public async Task<LoginResponse> AuthenticateCustomerAsync(LoginRequestDto request)
        {
            // Find user
            var customer = await _customerRepo.GetByEmailAsync(request.Email);
            if (customer == null || customer.PasswordHash != HashPassword(request.Password))
            {
                return null; // Invalid login
            }

            // Claims
            var claims = new[]
            {
                 new Claim(ClaimTypes.Name, customer.Email), // 👈 This is what Identity.Name will read
        new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
        new Claim(ClaimTypes.Email, customer.Email),
        new Claim("PhoneNumber", customer.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
            new Claim("customerId", customer.Id.ToString()),
            new Claim(ClaimTypes.Role, "Customer")
        };

            // Signing key
            var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])
);

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            double duration = double.Parse(_configuration["JwtSettings:DurationInMinutes"]);
            // Token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(duration),
                signingCredentials: creds
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = DateTime.Now.AddMinutes(duration)
            };
        }
        public async Task<bool> VerifyOtpAsync(Guid customerId, string code)
        {
            var otp = await _db.Otps.FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Code == code && !o.IsUsed && o.ExpiresAt > DateTime.UtcNow);
            if (otp == null) return false;
            otp.IsUsed = true;
            var cust = await _db.Customers.FindAsync(customerId);
            if (cust == null) return false;
            cust.IsPhoneVerified = true;
            await _db.SaveChangesAsync();
            return true;
        }

        private static string GenerateOtp()
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var num = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return num.ToString("D6");
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
