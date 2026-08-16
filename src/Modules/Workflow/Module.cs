using SmartSchool.Modules.Workflow.Persistence;
using FluentValidation;
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

        services.AddScoped<CreateApproval.Handler>();
        services.AddScoped<GetApprovalById.Handler>();
        services.AddScoped<GetApprovalPage.Handler>();
        services.AddScoped<UpdateApproval.Handler>();
        services.AddScoped<DeleteApproval.Handler>();
        services.AddScoped<IValidator<CreateApproval.Request>, CreateApproval.Validator>();
        services.AddScoped<IValidator<UpdateApproval.Request>, UpdateApproval.Validator>();
        services.AddScoped<CreateWorkflowDefinition.Handler>();
        services.AddScoped<GetWorkflowDefinitionById.Handler>();
        services.AddScoped<GetWorkflowDefinitionPage.Handler>();
        services.AddScoped<UpdateWorkflowDefinition.Handler>();
        services.AddScoped<DeleteWorkflowDefinition.Handler>();
        services.AddScoped<IValidator<CreateWorkflowDefinition.Request>, CreateWorkflowDefinition.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowDefinition.Request>, UpdateWorkflowDefinition.Validator>();
        services.AddScoped<CreateWorkflowInstance.Handler>();
        services.AddScoped<GetWorkflowInstanceById.Handler>();
        services.AddScoped<GetWorkflowInstancePage.Handler>();
        services.AddScoped<UpdateWorkflowInstance.Handler>();
        services.AddScoped<DeleteWorkflowInstance.Handler>();
        services.AddScoped<IValidator<CreateWorkflowInstance.Request>, CreateWorkflowInstance.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowInstance.Request>, UpdateWorkflowInstance.Validator>();
        services.AddScoped<CreateWorkflowStep.Handler>();
        services.AddScoped<GetWorkflowStepById.Handler>();
        services.AddScoped<GetWorkflowStepPage.Handler>();
        services.AddScoped<UpdateWorkflowStep.Handler>();
        services.AddScoped<DeleteWorkflowStep.Handler>();
        services.AddScoped<IValidator<CreateWorkflowStep.Request>, CreateWorkflowStep.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowStep.Request>, UpdateWorkflowStep.Validator>();

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
