using DocumentApproval.Common;
using DocumentApproval.Dtos;
using Elsa.Activities.Signaling.Services;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public class WorkflowService: IWorkflowService
    {
        private readonly ISignaler _signaler;
        private readonly IDocumentApprovalService _documentApprovalService;

        public WorkflowService(ISignaler signaler, IDocumentApprovalService documentApprovalService)
        {
            _signaler = signaler;
            _documentApprovalService = documentApprovalService;
        }

        public async Task<IEnumerable<CollectedWorkflow>> StartWorkflow(SignalDto signalDto)
        {
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Create{signalDto.DocumentType}Document",
                input: signalDto
            );

            return startedWorkFlows;
        }

        public async Task<IEnumerable<CollectedWorkflow>> ApproveWorkflow(SignalDto signalDto)
        {
            var document = _documentApprovalService.GetDocumentApprovalByDocumentId(signalDto.DocumentId!);
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Approve{signalDto.DocumentType}Document",
                input: signalDto,
                workflowInstanceId: document!.WorkflowInstanceId
            );

            return startedWorkFlows;
        }

        public async Task<IEnumerable<CollectedWorkflow>> RejectWorkflow(SignalDto signalDto)
        {
            var document = _documentApprovalService.GetDocumentApprovalByDocumentId(signalDto.DocumentId!);
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Reject{signalDto.DocumentType}Document",
                input: signalDto,
                workflowInstanceId: document!.WorkflowInstanceId
            );

            return startedWorkFlows;
        }
    }
}
