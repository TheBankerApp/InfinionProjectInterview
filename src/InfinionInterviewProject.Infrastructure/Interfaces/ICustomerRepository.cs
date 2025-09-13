using InfinionInterviewProject.Domain.Entities;

namespace InfinionInterviewProject.Infrastructure.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer> GetByEmailAsync(string email);
    }
}
