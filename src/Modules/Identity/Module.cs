using SmartSchool.Modules.Identity.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateRoleAssignment.Request>, CreateRoleAssignment.Validator>();
        services.AddScoped<IValidator<UpdateRoleAssignment.Request>, UpdateRoleAssignment.Validator>();
        services.AddScoped<IValidator<CreateUserProfile.Request>, CreateUserProfile.Validator>();
        services.AddScoped<IValidator<UpdateUserProfile.Request>, UpdateUserProfile.Validator>();


        services.AddScoped<IRequestHandler<CreateRoleAssignment.Request, Result<RoleAssignmentResponse>>, CreateRoleAssignment.Handler>();
        services.AddScoped<IRequestHandler<GetRoleAssignmentById.Query, Result<RoleAssignmentResponse>>, GetRoleAssignmentById.Handler>();
        services.AddScoped<IRequestHandler<GetRoleAssignmentPage.Query, Result<PagedResult<RoleAssignmentResponse>>>, GetRoleAssignmentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateRoleAssignment.Request, Result<RoleAssignmentResponse>>, UpdateRoleAssignment.Handler>();
        services.AddScoped<IRequestHandler<DeleteRoleAssignment.Command, Result<DeleteRoleAssignment.Response>>, DeleteRoleAssignment.Handler>();
        services.AddScoped<IRequestHandler<CreateUserProfile.Request, Result<UserProfileResponse>>, CreateUserProfile.Handler>();
        services.AddScoped<IRequestHandler<GetUserProfileById.Query, Result<UserProfileResponse>>, GetUserProfileById.Handler>();
        services.AddScoped<IRequestHandler<GetUserProfilePage.Query, Result<PagedResult<UserProfileResponse>>>, GetUserProfilePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateUserProfile.Request, Result<UserProfileResponse>>, UpdateUserProfile.Handler>();
        services.AddScoped<IRequestHandler<DeleteUserProfile.Command, Result<DeleteUserProfile.Response>>, DeleteUserProfile.Handler>();

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
