namespace DocumentApproval.Dtos
{
    public class Signal
    {
        public string? DocumentId { get; set; }
        public string? DocumentType { get; set; }
        public string? Role { get; set; }
        public string? Payload { get; set; }
        public string? WorkflowDefinitionId { get; set; }
    }
}
