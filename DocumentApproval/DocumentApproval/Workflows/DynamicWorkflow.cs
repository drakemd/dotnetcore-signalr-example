using DocumentApproval.Activities;
using DocumentApproval.Common;
using Elsa;
using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Primitives;
using Elsa.Activities.Temporal;
using Elsa.Builders;
using NodaTime;

namespace DocumentApproval.Workflows
{
    public class DynamicWorkflow: IWorkflow
    {
        public string DocumentType { get; set; }
        public List<string> ApproverList { get; set; }

        public void Build(IWorkflowBuilder builder)
        {
            builder
                .SignalReceived($"Create{DocumentType}Document")
                .Then<CreateDocumentActivity>()
                .SetVariable<int>("ApprovalCount", 0)
                .While(context => context.GetVariable<int>("ApprovalCount") < ApproverList.Count, @body =>
                {
                    @body
                    .SetVariable("nextApprover", context => ApproverList[context.GetVariable<int>("ApprovalCount")])
                    .WithName("Fork")
                    .Then<Fork>
                    (
                        activity => activity.WithBranches("Approve", "Reject", "Remind"),
                        fork =>
                        {
                            fork
                                .When("Approve")
                                .SignalReceived($"Approve{DocumentType}Document")
                                .Then<ApproveDocumentActivity>()
                                .SetVariable("ApprovalCount", context => context.GetVariable<int>("ApprovalCount") + 1)
                                .If(context => context.GetVariable<string>("ApprovalResult") == "Approved", @if =>
                                {
                                    @if.When(OutcomeNames.True).ThenNamed("Join");
                                    @if.When(OutcomeNames.False).ThenNamed("Fork");
                                });
                            fork
                                .When("Reject")
                                .SignalReceived($"Reject{DocumentType}Document")
                                .Then<RejectDocumentActivity>()
                                .SetVariable("ApprovalCount", context => context.GetVariable<int>("ApprovalCount") + 1)
                                .If(context => context.GetVariable<string>("RejectionResult") == "Rejected", @if =>
                                {
                                    @if.When(OutcomeNames.True).Then<Finish>();
                                    @if.When(OutcomeNames.False).ThenNamed("Fork");
                                });
                            fork
                                .When("Remind")
                                .Timer(Duration.FromSeconds(10)).WithName("Reminder")
                                .Then<RemindDocumentActivity>()
                                .ThenNamed("Reminder1");
                        }
                    );
                })
                .Then<Finish>();
        }
    }
}
