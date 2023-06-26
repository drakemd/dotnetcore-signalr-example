using DocumentApproval.Dtos;
using DocumentApproval.Services;
using Elsa.Services;
using Elsa.Attributes;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using Newtonsoft.Json;

namespace DocumentApproval.Activities
{
    [Activity(Category = "Document Approval", Description = "Reject the specified document.")]
    public class RejectDocumentActivity: Activity
    {
        private readonly ILogger<RejectDocumentActivity> _logger;
        private readonly IDocumentApprovalService _documentApprovalService;

        public RejectDocumentActivity(ILogger<RejectDocumentActivity> logger, IDocumentApprovalService documentApprovalService)
        {
            _logger = logger;
            _documentApprovalService = documentApprovalService;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var stringVal = JsonConvert.SerializeObject(context.Input);
            var signal = JsonConvert.DeserializeObject<SignalDto>(stringVal);

            var document = _documentApprovalService.GetDocumentApprovalByWorkflowInstanceId(context.WorkflowInstance.Id);

            if (signal.Role != document.ApproverRole)
            {
                _logger.LogWarning($"Permission Error");
                context.WorkflowExecutionContext.SetVariable("RejectionResult", "PermissionError");
                return Done();
            };

            document.Status = "Rejected";
            document.StatusMessage = $"Rejected by {document.ApproverRole}";
            document.LastUpdatedDate = DateTime.Now;

            _documentApprovalService.UpdateDocumentApproval(document);
            context.WorkflowExecutionContext.SetVariable("RejectionResult", "Rejected");

            // TODO: publish event workflow rejected
            _logger.LogWarning($"rejected document with id: {document.DocumentId}");

            return Done();
        }
    }
}
