using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Features.Activity;
using SmartSchool.Modules.Activities.Features.Award;
using SmartSchool.Modules.Activities.Features.StudentActivity;
using SmartSchool.Modules.Activities.Features.StudentOfMonth;
using SmartSchool.Modules.Activities.Persistence;

namespace SmartSchool.Modules.Activities;

public static class Module
{
    public static IServiceCollection AddActivitiesModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddModuleDbContext<ActivitiesDbContext, IActivitiesDbContext>(
            configuration,
            ModuleConstants.Schema);
        services.AddFeaturePersistence(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapActivitiesEndpoints(this IEndpointRouteBuilder endpoints)
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
        GetStudentActivityPage.MapEndpoint(endpoints);
        UpdateStudentActivity.MapEndpoint(endpoints);
        DeleteStudentActivity.MapEndpoint(endpoints);

        CreateStudentOfMonth.MapEndpoint(endpoints);
        DeleteStudentOfMonth.MapEndpoint(endpoints);
        GetStudentOfMonthById.MapEndpoint(endpoints);
        GetStudentOfMonthPage.MapEndpoint(endpoints);
        UpdateStudentOfMonth.MapEndpoint(endpoints);

        return endpoints;
    }
}
