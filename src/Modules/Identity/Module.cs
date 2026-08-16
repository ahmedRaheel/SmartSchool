using SmartSchool.Modules.Identity.Persistence;
using FluentValidation;
using SmartSchool.Modules.Identity.Features.RoleAssignment;
using SmartSchool.Modules.Identity.Features.UserProfile;

namespace SmartSchool.Modules.Identity;

public static class Module
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services)
    {
        services.AddScoped<IRoleAssignmentQuery, RoleAssignmentQuery>();
        services.AddScoped<IRoleAssignmentCommand, RoleAssignmentCommand>();
        services.AddScoped<IUserProfileQuery, UserProfileQuery>();
        services.AddScoped<IUserProfileCommand, UserProfileCommand>();

        services.AddScoped<CreateRoleAssignment.Handler>();
        services.AddScoped<GetRoleAssignmentById.Handler>();
        services.AddScoped<GetRoleAssignmentPage.Handler>();
        services.AddScoped<UpdateRoleAssignment.Handler>();
        services.AddScoped<DeleteRoleAssignment.Handler>();
        services.AddScoped<IValidator<CreateRoleAssignment.Request>, CreateRoleAssignment.Validator>();
        services.AddScoped<IValidator<UpdateRoleAssignment.Request>, UpdateRoleAssignment.Validator>();
        services.AddScoped<CreateUserProfile.Handler>();
        services.AddScoped<GetUserProfileById.Handler>();
        services.AddScoped<GetUserProfilePage.Handler>();
        services.AddScoped<UpdateUserProfile.Handler>();
        services.AddScoped<DeleteUserProfile.Handler>();
        services.AddScoped<IValidator<CreateUserProfile.Request>, CreateUserProfile.Validator>();
        services.AddScoped<IValidator<UpdateUserProfile.Request>, UpdateUserProfile.Validator>();

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
