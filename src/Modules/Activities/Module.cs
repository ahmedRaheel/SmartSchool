using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Features.Activity;
using SmartSchool.Modules.Activities.Features.Award;
using SmartSchool.Modules.Activities.Features.StudentActivity;
using SmartSchool.Modules.Activities.Features.StudentOfMonth;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities;

public static class Module
{
	public static IServiceCollection AddActivitiesModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IActivityQuery, ActivityQuery>();
		services.AddScoped<IActivityCommand, ActivityCommand>();
		services.AddScoped<IAwardQuery, AwardQuery>();
		services.AddScoped<IAwardCommand, AwardCommand>();
		services.AddScoped<IStudentActivityQuery, StudentActivityQuery>();
		services.AddScoped<IStudentActivityCommand, StudentActivityCommand>();
		services.AddScoped<IStudentOfMonthQuery, StudentOfMonthQuery>();
		services.AddScoped<IStudentOfMonthCommand, StudentOfMonthCommand>();

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
