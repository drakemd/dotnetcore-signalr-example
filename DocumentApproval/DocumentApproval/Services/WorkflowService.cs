using DocumentApproval.Common;
using DocumentApproval.Dtos;
using Elsa;
using Elsa.Activities.Signaling.Services;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Serialization;
using Elsa.Services;
using Elsa.Services.Models;

namespace DocumentApproval.Services
{
    public class WorkflowService: IWorkflowService
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly IDocumentApprovalService _documentApprovalService;
        private readonly ISignaler _signaler;
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IWorkflowBlueprintMaterializer _workflowBlueprintMaterializer;
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;

        public WorkflowService(
            IContentSerializer contentSerializer,
            IDocumentApprovalService documentApprovalService,
            ISignaler signaler,
            IStartsWorkflow startsWorkflow,
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IWorkflowInstanceStore workflowInstanceStore,
            IWorkflowRegistry workflowRegistry)
        {
            _contentSerializer = contentSerializer;
            _documentApprovalService = documentApprovalService;
            _signaler = signaler;
            _startsWorkflow = startsWorkflow;
            _workflowBlueprintMaterializer = workflowBlueprintMaterializer;
            _workflowDefinitionStore = workflowDefinitionStore;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<IEnumerable<CollectedWorkflow>> StartWorkflow(Signal signalDto)
        {
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Create{signalDto.DocumentType}Document",
                input: signalDto
            );

            return startedWorkFlows;
        }

        public async Task<SignalResult> ApproveWorkflow(Signal signalDto)
        {
            var document = _documentApprovalService.GetDocumentApprovalByDocumentId(signalDto.DocumentId!);
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Approve{signalDto.DocumentType}Document",
                input: signalDto,
                workflowInstanceId: document!.WorkflowInstanceId
            );

            var specification = new WorkflowInstanceIdSpecification(document.WorkflowInstanceId);
            var workflow = await _workflowInstanceStore.FindAsync(specification);
            var approvalResult = workflow?.Variables.Get<string>("ApprovalResult");

            return new SignalResult
            {
                IsSuccess = approvalResult == ApprovalResult.Approved,
                Message = approvalResult ?? ""
            };
        }

        public async Task<SignalResult> RejectWorkflow(Signal signalDto)
        {
            var document = _documentApprovalService.GetDocumentApprovalByDocumentId(signalDto.DocumentId!);
            var startedWorkFlows = await _signaler.TriggerSignalAsync
            (
                $"Reject{signalDto.DocumentType}Document",
                input: signalDto,
                workflowInstanceId: document!.WorkflowInstanceId
            );

            var specification = new WorkflowInstanceIdSpecification(document.WorkflowInstanceId);
            var workflow = await _workflowInstanceStore.FindAsync(specification);
            var approvalResult = workflow?.Variables.Get<string>("ApprovalResult");

            return new SignalResult
            {
                IsSuccess = approvalResult == ApprovalResult.Rejected,
                Message = approvalResult ?? ""
            };
        }

        public async Task CreateWorkflow(Signal signalDto)
        {
            var workflowDefinition = new WorkflowDefinition
            {
                Id = "1",
                DefinitionId = signalDto.WorkflowDefinitionId,
                Version = 1,
                IsPublished = true,
                IsLatest = true,
                PersistenceBehavior = WorkflowPersistenceBehavior.WorkflowBurst,
                Activities = new[]
                {
                    WriteLine("write-line-1", "==Composite Activities Demo=="),
                    new CompositeActivityDefinition
                    {
                        ActivityId = "composite-1",
                        Activities = new[]
                        {
                            WriteLine("write-line-2", "Line 1 of composite activity."),
                            WriteLine("write-line-3", "Line 2 of composite activity."),
                        },
                        Connections = new[]
                        {
                            new ConnectionDefinition("write-line-2", "write-line-3", OutcomeNames.Done)
                        }
                    },
                    WriteLine("write-line-4", "==End=="),
                },
                Connections = new[]
                {
                    new ConnectionDefinition("write-line-1", "composite-1", OutcomeNames.Done),
                    new ConnectionDefinition("composite-1", "write-line-4", OutcomeNames.Done),
                }
            };

            await _workflowDefinitionStore.SaveAsync(workflowDefinition);
        }

        public async Task RunConsoleWorkflow(Signal signalDto)
        {
            var specification = new Elsa.Persistence.Specifications.WorkflowDefinitions.WorkflowDefinitionIdSpecification(signalDto.WorkflowDefinitionId);
            var workflowDefinition = await _workflowDefinitionStore.FindAsync(specification);

            // Serialize workflow definition to JSON.
            var json = _contentSerializer.Serialize(workflowDefinition);

            // Deserialize workflow definition from JSON.
            var deserializedWorkflowDefinition = _contentSerializer.Deserialize<WorkflowDefinition>(json);

            // Materialize workflow.
            var workflowBlueprint = await _workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(deserializedWorkflowDefinition);

            // Execute workflow.
            await _startsWorkflow.StartWorkflowAsync(workflowBlueprint);
        }

        private static ActivityDefinition WriteLine(string id, string text) =>
            new()
            {
                ActivityId = id,
                Type = nameof(Elsa.Activities.Console.WriteLine),
                Properties = new List<ActivityDefinitionProperty>()
                {
                    ActivityDefinitionProperty.Liquid(nameof(Elsa.Activities.Console.WriteLine.Text), text)
                }
            };
    }
}
