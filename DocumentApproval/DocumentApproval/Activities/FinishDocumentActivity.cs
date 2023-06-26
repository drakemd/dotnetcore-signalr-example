using Elsa.Services;
using Elsa.Attributes;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using DocumentApproval.Services;

namespace DocumentApproval.Activities
{
    [Activity(Category = "Document Approval", Description = "Finish the document workflow.")]
    public class FinishDocumentActivity: Activity
    {
        private readonly ILogger<FinishDocumentActivity> _logger;
        private readonly IDocumentApprovalService _documentApprovalService;

        public FinishDocumentActivity(ILogger<FinishDocumentActivity> logger, IDocumentApprovalService documentApprovalService)
        {
            _logger = logger;
            _documentApprovalService = documentApprovalService;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var document = _documentApprovalService.GetDocumentApprovalByWorkflowInstanceId(context.WorkflowInstance.Id);
            document.Status = "Completed";
            document.StatusMessage = "Workflow Completed";
            document.LastUpdatedDate = DateTime.Now;

            _documentApprovalService.UpdateDocumentApproval(document);

            // TODO: publish event workflow finished
            _logger.LogWarning($"Finishing workflow document with id: {document.DocumentId}");

            return Done();
        }
    }
}
