using Elsa.Services;
using Elsa.Attributes;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using DocumentApproval.Services;
using DocumentApproval.Dtos;
using Newtonsoft.Json;
using Elsa;

namespace DocumentApproval.Activities
{
    [Activity(Category = "Document Approval", Description = "Approve the specified document.")]
    public class ApproveDocumentActivity: Activity
    {
        private readonly ILogger<ApproveDocumentActivity> _logger;
        private readonly IDocumentApprovalService _documentApprovalService;

        public ApproveDocumentActivity(ILogger<ApproveDocumentActivity> logger, IDocumentApprovalService documentApprovalService)
        {
            _logger = logger;
            _documentApprovalService = documentApprovalService;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var stringVal = JsonConvert.SerializeObject(context.Input);
            var signal = JsonConvert.DeserializeObject<SignalDto>(stringVal);

            var nextApprover = context.WorkflowExecutionContext.GetVariable<string>("nextApprover");
            var document =_documentApprovalService.GetDocumentApprovalByWorkflowInstanceId(context.WorkflowInstance.Id);

            if (signal.Role != document.ApproverRole) {
                _logger.LogWarning($"Permission Error");
                context.WorkflowExecutionContext.SetVariable("ApprovalResult", "PermissionError");
                return Done();
            };

            document.ApproverRole = nextApprover;
            document.StatusMessage = $"Waiting {nextApprover} Approval";
            document.LastUpdatedDate = DateTime.Now;

            _documentApprovalService.UpdateDocumentApproval(document);
            context.WorkflowExecutionContext.SetVariable("ApprovalResult", "Approved");

            // TODO: publish event workflow approved
            _logger.LogWarning($"approved document with id: {document.DocumentId}");

            return Done();
        }
    }
}
