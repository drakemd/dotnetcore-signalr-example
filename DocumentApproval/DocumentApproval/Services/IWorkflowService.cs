using DocumentApproval.Dtos;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public interface IWorkflowService
    {
        public Task<IEnumerable<CollectedWorkflow>> StartWorkflow(SignalDto signalDto);
        public Task<IEnumerable<CollectedWorkflow>> ApproveWorkflow(SignalDto signalDto);
        public Task<IEnumerable<CollectedWorkflow>> RejectWorkflow(SignalDto signalDto);
    }
}
