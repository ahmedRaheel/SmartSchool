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

        services.AddScoped<CreateActivity.Handler>();
        services.AddScoped<GetActivityById.Handler>();
        services.AddScoped<GetActivityPage.Handler>();
        services.AddScoped<UpdateActivity.Handler>();
        services.AddScoped<DeleteActivity.Handler>();
        services.AddScoped<IValidator<CreateActivity.Request>, CreateActivity.Validator>();
        services.AddScoped<IValidator<UpdateActivity.Request>, UpdateActivity.Validator>();
        services.AddScoped<CreateAward.Handler>();
        services.AddScoped<GetAwardById.Handler>();
        services.AddScoped<GetAwardPage.Handler>();
        services.AddScoped<UpdateAward.Handler>();
        services.AddScoped<DeleteAward.Handler>();
        services.AddScoped<IValidator<CreateAward.Request>, CreateAward.Validator>();
        services.AddScoped<IValidator<UpdateAward.Request>, UpdateAward.Validator>();
        services.AddScoped<CreateStudentActivity.Handler>();
        services.AddScoped<GetStudentActivityById.Handler>();
        services.AddScoped<GetStudentActivityPage.Handler>();
        services.AddScoped<UpdateStudentActivity.Handler>();
        services.AddScoped<DeleteStudentActivity.Handler>();
        services.AddScoped<IValidator<CreateStudentActivity.Request>, CreateStudentActivity.Validator>();
        services.AddScoped<IValidator<UpdateStudentActivity.Request>, UpdateStudentActivity.Validator>();
        services.AddScoped<CreateStudentOfMonth.Handler>();
        services.AddScoped<GetStudentOfMonthById.Handler>();
        services.AddScoped<GetStudentOfMonthPage.Handler>();
        services.AddScoped<UpdateStudentOfMonth.Handler>();
        services.AddScoped<DeleteStudentOfMonth.Handler>();
        services.AddScoped<IValidator<CreateStudentOfMonth.Request>, CreateStudentOfMonth.Validator>();
        services.AddScoped<IValidator<UpdateStudentOfMonth.Request>, UpdateStudentOfMonth.Validator>();

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
