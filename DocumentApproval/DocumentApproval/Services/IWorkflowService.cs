using DocumentApproval.Dtos;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public interface IWorkflowService
    {
        public Task<IEnumerable<CollectedWorkflow>> StartWorkflow(Signal signalDto);
        public Task<SignalResult> ApproveWorkflow(Signal signalDto);
        public Task<SignalResult> RejectWorkflow(Signal signalDto);
        public Task CreateWorkflow(Signal signalDto);
        public Task RunConsoleWorkflow(Signal signalDto);
    }
}
