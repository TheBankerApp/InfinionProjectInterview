using System.ComponentModel.DataAnnotations.Schema;

namespace InfinionInterviewProject.Domain.Entities
{
    [Table("tbl_Customers")]
    public class Customer
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string State { get; set; }
            public string Lga { get; set; }
            public bool IsPhoneVerified { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}