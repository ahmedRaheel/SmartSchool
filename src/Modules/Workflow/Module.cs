using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;

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
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeatureDataAccess(typeof(Module).Assembly);
		return services;
	}

	public static IEndpointRouteBuilder MapWorkflowEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		CreateApproval.MapEndpoint(endpoints);
		CreateWorkflowDefinition.MapEndpoint(endpoints);
		CreateWorkflowInstance.MapEndpoint(endpoints);
		CreateWorkflowStep.MapEndpoint(endpoints);
		DeleteApproval.MapEndpoint(endpoints);
		DeleteWorkflowDefinition.MapEndpoint(endpoints);
		DeleteWorkflowInstance.MapEndpoint(endpoints);
		DeleteWorkflowStep.MapEndpoint(endpoints);
		GetApprovalById.MapEndpoint(endpoints);
		GetApprovalPage.MapEndpoint(endpoints);
		GetWorkflowDefinitionById.MapEndpoint(endpoints);
		GetWorkflowDefinitionPage.MapEndpoint(endpoints);
		GetWorkflowInstanceById.MapEndpoint(endpoints);
		GetWorkflowInstancePage.MapEndpoint(endpoints);
		GetWorkflowStepById.MapEndpoint(endpoints);
		GetWorkflowStepPage.MapEndpoint(endpoints);
		UpdateApproval.MapEndpoint(endpoints);
		UpdateWorkflowDefinition.MapEndpoint(endpoints);
		UpdateWorkflowInstance.MapEndpoint(endpoints);
		UpdateWorkflowStep.MapEndpoint(endpoints);

		return endpoints;
	}
}
