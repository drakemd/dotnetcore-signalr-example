using DocumentApproval.Activities;
using DocumentApproval.Common;
using Elsa;
using Elsa.Activities.ControlFlow;
using Elsa.Activities.Temporal;
using Elsa.Builders;
using NodaTime;

namespace DocumentApproval.Workflows
{
    public class ChangeApprovalWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .SignalReceived($"Create{DocumentType.ChangeApproval}Document")
                .Then<CreateDocumentActivity>()
                .WithName("Fork1")
                .Then<Fork>
                (
                    activity => activity.WithBranches("Approve", "Reject", "Remind"),
                    fork =>
                    {
                        fork
                            .When("Approve")
                            .SignalReceived($"Approve{DocumentType.ChangeApproval}Document")
                            .Then<ApproveDocumentActivity>()
                            .If(context => context.GetVariable<string>("ApprovalResult") == "Approved", @if =>
                            {
                                @if.When(OutcomeNames.True).ThenNamed("Join1");
                                @if.When(OutcomeNames.False).ThenNamed("Fork1");
                            });
                        fork
                            .When("Reject")
                            .SignalReceived($"Reject{DocumentType.ChangeApproval}Document")
                            .Then<RejectDocumentActivity>()
                            .If(context => context.GetVariable<string>("RejectionResult") == "Rejected", @if =>
                            {
                                @if.When(OutcomeNames.True).Then<Finish>();
                                @if.When(OutcomeNames.False).ThenNamed("Fork1");
                            });
                        fork
                            .When("Remind")
                            .Timer(Duration.FromSeconds(10)).WithName("Reminder1")
                            .Then<RemindDocumentActivity>()
                            .ThenNamed("Reminder1");
                    }
                )
                .Add<Join>(join => {
                    join
                        .WithEagerJoin(true)
                        .WithMode(Join.JoinMode.WaitAny);
                }).WithName("Join1")
                .Then<FinishDocumentActivity>()
                .Then<Finish>();
        }
    }
}
