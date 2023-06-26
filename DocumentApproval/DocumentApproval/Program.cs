using DocumentApproval.Activities;
using DocumentApproval.Persistence;
using DocumentApproval.Services;
using DocumentApproval.Workflows;
using Elsa;
using Elsa.Activities.Console;
using Elsa.Activities.Temporal.Quartz;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddFilter("Microsoft", LogLevel.Warning)
              .AddFilter("System", LogLevel.Warning)
              .AddFilter("Default", LogLevel.Warning)
              .AddFilter("Linq", LogLevel.Warning)
              .AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddElsa(options => 
    options
        .UseEntityFrameworkPersistence(ef => ef.UseSqlServer("Server=tcp:pocbuma.database.windows.net,1433;Initial Catalog=elsasample;Persist Security Info=False;User ID=bumaadmin;Password=Buma,12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        .AddQuartzTemporalActivities()
        .AddActivity<WriteLine>()
        .AddActivity<CreateDocumentActivity>()
        .AddActivity<ApproveDocumentActivity>()
        .AddActivity<RejectDocumentActivity>()
        .AddActivity<RemindDocumentActivity>()
        .AddActivity<FinishDocumentActivity>()
        .AddWorkflow<LeaveApprovalWorkflow>()
        .AddWorkflow<ChangeApprovalWorkflow>()
);
builder.Services.AddDbContext<DocumentApprovalContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IWorkflowService, WorkflowService>();
builder.Services.AddTransient<IDocumentApprovalService, DocumentApprovalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
