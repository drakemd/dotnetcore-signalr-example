using DocumentApproval.Dtos;
using DocumentApproval.Persistence;
using Elsa.Activities.Workflows.Workflow;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public class DocumentApprovalService: IDocumentApprovalService
    {
        private readonly DocumentApprovalContext _documentApprovalContext;

        public DocumentApprovalService(DocumentApprovalContext documentApprovalContext)
        {
            _documentApprovalContext = documentApprovalContext;
        }

        public Persistence.Models.DocumentApproval? GetDocumentApprovalByDocumentId(string id)
        {
            var document = _documentApprovalContext
                .DocumentApprovals
                .Where(q => q.DocumentId == id)
                .FirstOrDefault();
            return document;
        }

        public Persistence.Models.DocumentApproval? GetDocumentApprovalByWorkflowInstanceId(string id)
        {
            var document =_documentApprovalContext
                .DocumentApprovals
                .Where(q => q.WorkflowInstanceId == id)
                .FirstOrDefault();
            return document;
        }

        public void CreateDocumentApproval(Signal signalDto, string workflowInstanceId, string firstApprover)
        {
            List<Persistence.Models.DocumentApproval> documentApprovals = new List<Persistence.Models.DocumentApproval>();

            documentApprovals.Add(new Persistence.Models.DocumentApproval
            {
                WorkflowInstanceId = workflowInstanceId,
                DocumentId = signalDto.DocumentId!,
                DocumentType = signalDto.DocumentType!,
                Payload = signalDto.Payload!,
                ApproverRole = firstApprover,
                Status = "Inprogress",
                StatusMessage = $"Waiting {firstApprover} Approval",
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
            });

            _documentApprovalContext.DocumentApprovals.AddRange(documentApprovals);
            _documentApprovalContext.SaveChanges();
        }

        public void UpdateDocumentApproval(Persistence.Models.DocumentApproval documentApproval)
        {
            _documentApprovalContext.Update(documentApproval);
            _documentApprovalContext.SaveChanges();
        }
    }
}
