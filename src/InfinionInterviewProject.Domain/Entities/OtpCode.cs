namespace InfinionInterviewProject.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace InfinionInterviewProject.Domain.Entities
    {
        [Table("tbl_OtpCodes")]
        public class OtpCode
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid CustomerId { get; set; }
            public string Code { get; set; }
            public DateTime ExpiresAt { get; set; }
            public bool IsUsed { get; set; }
        }
    }

}
