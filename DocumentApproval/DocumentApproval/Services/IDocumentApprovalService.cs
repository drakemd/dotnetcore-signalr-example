using DocumentApproval.Dtos;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public interface IDocumentApprovalService
    {
        public Persistence.Models.DocumentApproval? GetDocumentApprovalByWorkflowInstanceId(string id);
        public Persistence.Models.DocumentApproval? GetDocumentApprovalByDocumentId(string id);
        public void CreateDocumentApproval(Signal signalDto, string workflowInstanceId, string firstApprover);
        public void UpdateDocumentApproval(Persistence.Models.DocumentApproval documentApproval);
    }
}
