namespace InfinionInterviewProject.Domain.Common
{
    public class AuditableEntity
    {
        public DateTime? CreateDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? AuthDate { get; set; }
        public int? AuthorizedBy { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerfiedDate { get; set; }
        public int? RejectedBy { get; set; }
        public DateTime? RejectDate { get; set; }
    }
}
