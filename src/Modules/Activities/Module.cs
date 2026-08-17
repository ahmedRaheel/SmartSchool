using SmartSchool.Modules.Activities.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Persistence;
using FluentValidation;
using SmartSchool.Modules.Activities.Features.Activity;
using SmartSchool.Modules.Activities.Features.Award;
using SmartSchool.Modules.Activities.Features.StudentActivity;
using SmartSchool.Modules.Activities.Features.StudentOfMonth;

namespace SmartSchool.Modules.Activities;

public static class Module
{
    public static IServiceCollection AddActivitiesModule(
        this IServiceCollection services)
    {
        services.AddScoped<IActivityQuery, ActivityQuery>();
        services.AddScoped<IActivityCommand, ActivityCommand>();
        services.AddScoped<IAwardQuery, AwardQuery>();
        services.AddScoped<IAwardCommand, AwardCommand>();
        services.AddScoped<IStudentActivityQuery, StudentActivityQuery>();
        services.AddScoped<IStudentActivityCommand, StudentActivityCommand>();
        services.AddScoped<IStudentOfMonthQuery, StudentOfMonthQuery>();
        services.AddScoped<IStudentOfMonthCommand, StudentOfMonthCommand>();
        services.AddScoped<IValidator<CreateActivity.Request>, CreateActivity.Validator>();
        services.AddScoped<IValidator<UpdateActivity.Request>, UpdateActivity.Validator>();
        services.AddScoped<IValidator<CreateAward.Request>, CreateAward.Validator>();
        services.AddScoped<IValidator<UpdateAward.Request>, UpdateAward.Validator>();
        services.AddScoped<IValidator<CreateStudentActivity.Request>, CreateStudentActivity.Validator>();
        services.AddScoped<IValidator<UpdateStudentActivity.Request>, UpdateStudentActivity.Validator>();
        services.AddScoped<IValidator<CreateStudentOfMonth.Request>, CreateStudentOfMonth.Validator>();
        services.AddScoped<IValidator<UpdateStudentOfMonth.Request>, UpdateStudentOfMonth.Validator>();


        services.AddScoped<IRequestHandler<CreateActivity.Request, Result<ActivityResponse>>, CreateActivity.Handler>();
        services.AddScoped<IRequestHandler<GetActivityById.Query, Result<ActivityResponse>>, GetActivityById.Handler>();
        services.AddScoped<IRequestHandler<GetActivityPage.Query, Result<PagedResult<ActivityResponse>>>, GetActivityPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateActivity.Request, Result<ActivityResponse>>, UpdateActivity.Handler>();
        services.AddScoped<IRequestHandler<DeleteActivity.Command, Result<DeleteActivity.Response>>, DeleteActivity.Handler>();
        services.AddScoped<IRequestHandler<CreateAward.Request, Result<AwardResponse>>, CreateAward.Handler>();
        services.AddScoped<IRequestHandler<GetAwardById.Query, Result<AwardResponse>>, GetAwardById.Handler>();
        services.AddScoped<IRequestHandler<GetAwardPage.Query, Result<PagedResult<AwardResponse>>>, GetAwardPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAward.Request, Result<AwardResponse>>, UpdateAward.Handler>();
        services.AddScoped<IRequestHandler<DeleteAward.Command, Result<DeleteAward.Response>>, DeleteAward.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentActivity.Request, Result<StudentActivityResponse>>, CreateStudentActivity.Handler>();
        services.AddScoped<IRequestHandler<GetStudentActivityById.Query, Result<StudentActivityResponse>>, GetStudentActivityById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentActivityPage.Query, Result<PagedResult<StudentActivityResponse>>>, GetStudentActivityPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentActivity.Request, Result<StudentActivityResponse>>, UpdateStudentActivity.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentActivity.Command, Result<DeleteStudentActivity.Response>>, DeleteStudentActivity.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentOfMonth.Request, Result<StudentOfMonthResponse>>, CreateStudentOfMonth.Handler>();
        services.AddScoped<IRequestHandler<GetStudentOfMonthById.Query, Result<StudentOfMonthResponse>>, GetStudentOfMonthById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentOfMonthPage.Query, Result<PagedResult<StudentOfMonthResponse>>>, GetStudentOfMonthPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentOfMonth.Request, Result<StudentOfMonthResponse>>, UpdateStudentOfMonth.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentOfMonth.Command, Result<DeleteStudentOfMonth.Response>>, DeleteStudentOfMonth.Handler>();

        return services;
    }

    public static IEndpointRouteBuilder MapActivitiesEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateActivity.MapEndpoint(endpoints);
        GetActivityById.MapEndpoint(endpoints);
        GetActivityPage.MapEndpoint(endpoints);
        UpdateActivity.MapEndpoint(endpoints);
        DeleteActivity.MapEndpoint(endpoints);
        CreateAward.MapEndpoint(endpoints);
        GetAwardById.MapEndpoint(endpoints);
        GetAwardPage.MapEndpoint(endpoints);
        UpdateAward.MapEndpoint(endpoints);
        DeleteAward.MapEndpoint(endpoints);
        CreateStudentActivity.MapEndpoint(endpoints);
        GetStudentActivityById.MapEndpoint(endpoints);
        GetStudentActivityPage.MapEndpoint(endpoints);
        UpdateStudentActivity.MapEndpoint(endpoints);
        DeleteStudentActivity.MapEndpoint(endpoints);
        CreateStudentOfMonth.MapEndpoint(endpoints);
        GetStudentOfMonthById.MapEndpoint(endpoints);
        GetStudentOfMonthPage.MapEndpoint(endpoints);
        UpdateStudentOfMonth.MapEndpoint(endpoints);
        DeleteStudentOfMonth.MapEndpoint(endpoints);

        return endpoints;
    }
}
