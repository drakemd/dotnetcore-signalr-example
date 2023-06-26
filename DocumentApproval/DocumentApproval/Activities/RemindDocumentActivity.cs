using Elsa.Services;
using Elsa.Attributes;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using DocumentApproval.Services;

namespace DocumentApproval.Activities
{
    [Activity(Category = "Document Approval", Description = "Remind the approver of the document.")]
    public class RemindDocumentActivity: Activity
    {
        private readonly ILogger<RemindDocumentActivity> _logger;
        private readonly IDocumentApprovalService _documentApprovalService;

        public RemindDocumentActivity(
            ILogger<RemindDocumentActivity> logger,
            IDocumentApprovalService documentApprovalService)
        {
            _logger = logger;
            _documentApprovalService = documentApprovalService;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var document =_documentApprovalService.GetDocumentApprovalByWorkflowInstanceId(context.WorkflowInstance.Id);
            _logger.LogWarning($"Remind document to ${document!.ApproverRole}");
            return Done();
        }
    }
}
