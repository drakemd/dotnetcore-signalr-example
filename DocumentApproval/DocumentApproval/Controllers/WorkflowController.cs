using DocumentApproval.Common;
using DocumentApproval.Dtos;
using DocumentApproval.Services;
using Elsa.Activities.Signaling.Services;
using Elsa.Models;
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
        public async Task<IActionResult> StartWorkflow([FromBody] Signal body)
        {
            var collectedWorkflows = await _workflowService.StartWorkflow(body);
            return new OkObjectResult(collectedWorkflows);
        }

        [HttpPost(Name = "ApproveWorkflow")]
        [ActionName("Approve")]
        public async Task<IActionResult> ApproveWorkflow([FromBody] Signal body)
        {
            var result = await _workflowService.ApproveWorkflow(body);

            if(!result.IsSuccess) return new BadRequestObjectResult(result);
            return new OkObjectResult(result);
        }

        [HttpPost(Name = "RejectWorkflow")]
        [ActionName("Reject")]
        public async Task<IActionResult> RejectWorkflow([FromBody] Signal body)
        {
            var result = await _workflowService.RejectWorkflow(body);
            if (!result.IsSuccess) return new BadRequestObjectResult(result);
            return new OkObjectResult(result);
        }

        [HttpPost(Name = "Create")]
        [ActionName("Create")]
        public async Task<IActionResult> CreateWorkflow([FromBody] Signal body)
        {
            await _workflowService.CreateWorkflow(body);
            return new OkResult();
        }

        [HttpPost(Name = "RunConsoleWorkflow")]
        [ActionName("RunConsoleWorkflow")]
        public async Task<IActionResult> RunConsoleWorkflow([FromBody] Signal body)
        {
            await _workflowService.RunConsoleWorkflow(body);
            return new OkResult();
        }
    }
}
