using SmartSchool.Application;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Features.Approval;
using SmartSchool.Modules.Workflow.Features.WorkflowDefinition;
using SmartSchool.Modules.Workflow.Features.WorkflowInstance;
using SmartSchool.Modules.Workflow.Persistence;
namespace SmartSchool.Modules.Workflow;
public static class Module
{
 public static IServiceCollection AddWorkflowModule(this IServiceCollection services){services.AddSmartSchoolMediator(typeof(Module).Assembly);services.AddScoped<IWorkflowDbContext>(sp=>sp.GetRequiredService<WorkflowDbContext>());services.AddFeaturePersistence(typeof(Module).Assembly);return services;}
 public static IEndpointRouteBuilder MapWorkflowEndpoints(this IEndpointRouteBuilder endpoints){CreateWorkflowDefinition.MapEndpoint(endpoints);GetWorkflowDefinitionPage.MapEndpoint(endpoints);UpdateWorkflowDefinition.MapEndpoint(endpoints);DeleteWorkflowDefinition.MapEndpoint(endpoints);CreateWorkflowInstance.MapEndpoint(endpoints);GetWorkflowInstancePage.MapEndpoint(endpoints);GetApprovalPage.MapEndpoint(endpoints);UpdateApproval.MapEndpoint(endpoints);return endpoints;}
}
