using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Features.RoleAssignment;
using SmartSchool.Modules.Identity.Features.UserProfile;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity;

public static class Module
{
	public static IServiceCollection AddIdentityModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IRoleAssignmentQuery, RoleAssignmentQuery>();
		services.AddScoped<IRoleAssignmentCommand, RoleAssignmentCommand>();
		services.AddScoped<IUserProfileQuery, UserProfileQuery>();
		services.AddScoped<IUserProfileCommand, UserProfileCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapIdentityEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateRoleAssignment.MapEndpoint(endpoints);
		GetRoleAssignmentById.MapEndpoint(endpoints);
		GetRoleAssignmentPage.MapEndpoint(endpoints);
		UpdateRoleAssignment.MapEndpoint(endpoints);
		DeleteRoleAssignment.MapEndpoint(endpoints);
		CreateUserProfile.MapEndpoint(endpoints);
		GetUserProfileById.MapEndpoint(endpoints);
		GetUserProfilePage.MapEndpoint(endpoints);
		UpdateUserProfile.MapEndpoint(endpoints);
		DeleteUserProfile.MapEndpoint(endpoints);

		return endpoints;
	}
}
