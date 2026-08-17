using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateApproval.Request>, CreateApproval.Validator>();
        services.AddScoped<IValidator<UpdateApproval.Request>, UpdateApproval.Validator>();
        services.AddScoped<IValidator<CreateWorkflowDefinition.Request>, CreateWorkflowDefinition.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowDefinition.Request>, UpdateWorkflowDefinition.Validator>();
        services.AddScoped<IValidator<CreateWorkflowInstance.Request>, CreateWorkflowInstance.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowInstance.Request>, UpdateWorkflowInstance.Validator>();
        services.AddScoped<IValidator<CreateWorkflowStep.Request>, CreateWorkflowStep.Validator>();
        services.AddScoped<IValidator<UpdateWorkflowStep.Request>, UpdateWorkflowStep.Validator>();


        services.AddScoped<IRequestHandler<CreateApproval.Request, Result<ApprovalResponse>>, CreateApproval.Handler>();
        services.AddScoped<IRequestHandler<GetApprovalById.Query, Result<ApprovalResponse>>, GetApprovalById.Handler>();
        services.AddScoped<IRequestHandler<GetApprovalPage.Query, Result<PagedResult<ApprovalResponse>>>, GetApprovalPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateApproval.Request, Result<ApprovalResponse>>, UpdateApproval.Handler>();
        services.AddScoped<IRequestHandler<DeleteApproval.Command, Result<DeleteApproval.Response>>, DeleteApproval.Handler>();
        services.AddScoped<IRequestHandler<CreateWorkflowDefinition.Request, Result<WorkflowDefinitionResponse>>, CreateWorkflowDefinition.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowDefinitionById.Query, Result<WorkflowDefinitionResponse>>, GetWorkflowDefinitionById.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowDefinitionPage.Query, Result<PagedResult<WorkflowDefinitionResponse>>>, GetWorkflowDefinitionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateWorkflowDefinition.Request, Result<WorkflowDefinitionResponse>>, UpdateWorkflowDefinition.Handler>();
        services.AddScoped<IRequestHandler<DeleteWorkflowDefinition.Command, Result<DeleteWorkflowDefinition.Response>>, DeleteWorkflowDefinition.Handler>();
        services.AddScoped<IRequestHandler<CreateWorkflowInstance.Request, Result<WorkflowInstanceResponse>>, CreateWorkflowInstance.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowInstanceById.Query, Result<WorkflowInstanceResponse>>, GetWorkflowInstanceById.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowInstancePage.Query, Result<PagedResult<WorkflowInstanceResponse>>>, GetWorkflowInstancePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateWorkflowInstance.Request, Result<WorkflowInstanceResponse>>, UpdateWorkflowInstance.Handler>();
        services.AddScoped<IRequestHandler<DeleteWorkflowInstance.Command, Result<DeleteWorkflowInstance.Response>>, DeleteWorkflowInstance.Handler>();
        services.AddScoped<IRequestHandler<CreateWorkflowStep.Request, Result<WorkflowStepResponse>>, CreateWorkflowStep.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowStepById.Query, Result<WorkflowStepResponse>>, GetWorkflowStepById.Handler>();
        services.AddScoped<IRequestHandler<GetWorkflowStepPage.Query, Result<PagedResult<WorkflowStepResponse>>>, GetWorkflowStepPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateWorkflowStep.Request, Result<WorkflowStepResponse>>, UpdateWorkflowStep.Handler>();
        services.AddScoped<IRequestHandler<DeleteWorkflowStep.Command, Result<DeleteWorkflowStep.Response>>, DeleteWorkflowStep.Handler>();

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
