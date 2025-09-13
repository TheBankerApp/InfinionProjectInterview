using InfinionInterviewProject.Domain.Entities;
using InfinionInterviewProject.Infrastructure.Interfaces;
using InfinionInterviewProject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace InfinionInterviewProject.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    { 
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }
        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
    }
}