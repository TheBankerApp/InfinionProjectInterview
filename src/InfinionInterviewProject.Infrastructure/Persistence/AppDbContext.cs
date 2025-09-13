using Microsoft.EntityFrameworkCore;
using InfinionInterviewProject.Domain.Entities;
using System;
using InfinionInterviewProject.Domain.Entities.InfinionInterviewProject.Domain.Entities;

namespace InfinionInterviewProject.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<OtpCode> Otps { get; set; }
        public DbSet<LgaSeed> Lgas { get; set; }
        public DbSet<StateSeed> States { get; set; }
    }

    public class LgaSeed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
    }

    public class StateSeed
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
