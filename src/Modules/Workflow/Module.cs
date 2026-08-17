
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Persistence;

using SmartSchool.Modules.Workflow.Features.Approval;
using SmartSchool.Modules.Workflow.Features.WorkflowDefinition;
using SmartSchool.Modules.Workflow.Features.WorkflowInstance;
using SmartSchool.Modules.Workflow.Features.WorkflowStep;

namespace SmartSchool.Modules.Workflow;

public static class Module
{
    public static IServiceCollection AddWorkflowModule(
        this IServiceCollection services)
    {
        services.AddScoped<IApprovalQuery, ApprovalQuery>();
        services.AddScoped<IApprovalCommand, ApprovalCommand>();
        services.AddScoped<IWorkflowDefinitionQuery, WorkflowDefinitionQuery>();
        services.AddScoped<IWorkflowDefinitionCommand, WorkflowDefinitionCommand>();
        services.AddScoped<IWorkflowInstanceQuery, WorkflowInstanceQuery>();
        services.AddScoped<IWorkflowInstanceCommand, WorkflowInstanceCommand>();
        services.AddScoped<IWorkflowStepQuery, WorkflowStepQuery>();
        services.AddScoped<IWorkflowStepCommand, WorkflowStepCommand>();

        return services;
    }

    public static IEndpointRouteBuilder MapWorkflowEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateApproval.MapEndpoint(endpoints);
        GetApprovalById.MapEndpoint(endpoints);
        GetApprovalPage.MapEndpoint(endpoints);
        UpdateApproval.MapEndpoint(endpoints);
        DeleteApproval.MapEndpoint(endpoints);
        CreateWorkflowDefinition.MapEndpoint(endpoints);
        GetWorkflowDefinitionById.MapEndpoint(endpoints);
        GetWorkflowDefinitionPage.MapEndpoint(endpoints);
        UpdateWorkflowDefinition.MapEndpoint(endpoints);
        DeleteWorkflowDefinition.MapEndpoint(endpoints);
        CreateWorkflowInstance.MapEndpoint(endpoints);
        GetWorkflowInstanceById.MapEndpoint(endpoints);
        GetWorkflowInstancePage.MapEndpoint(endpoints);
        UpdateWorkflowInstance.MapEndpoint(endpoints);
        DeleteWorkflowInstance.MapEndpoint(endpoints);
        CreateWorkflowStep.MapEndpoint(endpoints);
        GetWorkflowStepById.MapEndpoint(endpoints);
        GetWorkflowStepPage.MapEndpoint(endpoints);
        UpdateWorkflowStep.MapEndpoint(endpoints);
        DeleteWorkflowStep.MapEndpoint(endpoints);

        return endpoints;
    }
}
