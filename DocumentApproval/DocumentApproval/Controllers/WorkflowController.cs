using DocumentApproval.Common;
using DocumentApproval.Dtos;
using DocumentApproval.Services;
using Elsa.Activities.Signaling.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApproval.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpPost(Name = "StartWorkflow")]
        [ActionName("Start")]
        public async Task<IActionResult> StartWorkflow([FromBody] SignalDto body)
        {
            var collectedWorkflows = await _workflowService.StartWorkflow(body);
            return new OkObjectResult(collectedWorkflows);
        }

        [HttpPost(Name = "ApproveWorkflow")]
        [ActionName("Approve")]
        public async Task<IActionResult> ApproveWorkflow([FromBody] SignalDto body)
        {
            var collectedWorkflows = await _workflowService.ApproveWorkflow(body);
            return new OkObjectResult(collectedWorkflows);
        }

        [HttpPost(Name = "RejectWorkflow")]
        [ActionName("Reject")]
        public async Task<IActionResult> RejectWorkflow([FromBody] SignalDto body)
        {
            var collectedWorkflows = await _workflowService.RejectWorkflow(body);
            return new OkObjectResult(collectedWorkflows);
        }
    }
}
