using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentApproval.Persistence.Models
{
    public class DocumentApproval
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string? Payload { get; set; }
        public string ApproverRole { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
