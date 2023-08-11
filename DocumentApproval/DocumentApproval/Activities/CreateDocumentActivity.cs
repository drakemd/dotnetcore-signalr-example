using Elsa.Services;
using Elsa.Attributes;
using Elsa.ActivityResults;
using Elsa.Services.Models;
using DocumentApproval.Dtos;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using DocumentApproval.Common;
using DocumentApproval.Services;

namespace DocumentApproval.Activities
{
    [Activity(Category = "Document Approval", Description = "Create the specified document.")]
    public class CreateDocumentActivity: Activity
    {
        private readonly ILogger<CreateDocumentActivity> _logger;
        private readonly IDocumentApprovalService _documentApprovalService;

        public CreateDocumentActivity(ILogger<CreateDocumentActivity> logger, IDocumentApprovalService documentApprovalService) {
            _logger = logger;
            _documentApprovalService = documentApprovalService;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var stringVal = JsonConvert.SerializeObject(context.Input);
            var myType = JsonConvert.DeserializeObject<Signal>(stringVal);

            context.WorkflowExecutionContext.SetVariable("nextApprover", RoleLevel.Manager1);
            _documentApprovalService.CreateDocumentApproval
            (
                myType,
                context.WorkflowInstance.Id,
                RoleLevel.Manager1
            );

            // TODO: publish event workflow started
            _logger.LogWarning($"Creating workflow for document with id: {myType.DocumentId}");

            return Done();
        }
    }
}
