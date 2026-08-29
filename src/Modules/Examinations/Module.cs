using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Features.Exam;
using SmartSchool.Modules.Examinations.Features.ExamSubject;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Examinations.Features.GradeScale;
namespace SmartSchool.Modules.Examinations;

public static class Module
{
	public static IServiceCollection AddExaminationsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeaturePersistence(typeof(Module).Assembly);
		return services;
	}

	public static IEndpointRouteBuilder MapExaminationsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateExam.MapEndpoint(endpoints);
		GetExamById.MapEndpoint(endpoints);
		GetExamPage.MapEndpoint(endpoints);
		UpdateExam.MapEndpoint(endpoints);
		DeleteExam.MapEndpoint(endpoints);
		CreateExamSubject.MapEndpoint(endpoints);
		GetExamSubjectById.MapEndpoint(endpoints);
		GetExamSubjectPage.MapEndpoint(endpoints);
		UpdateExamSubject.MapEndpoint(endpoints);
		DeleteExamSubject.MapEndpoint(endpoints);
		CreateStudentExamResult.MapEndpoint(endpoints);
		GetStudentExamResultById.MapEndpoint(endpoints);
		GetStudentExamResultPage.MapEndpoint(endpoints);
		UpdateStudentExamResult.MapEndpoint(endpoints);
		DeleteStudentExamResult.MapEndpoint(endpoints);

		CreateGradeScale.MapEndpoint(endpoints);
		DeleteGradeScale.MapEndpoint(endpoints);
		GetGradeScaleById.MapEndpoint(endpoints);
		GetGradeScalePage.MapEndpoint(endpoints);
		UpdateGradeScale.MapEndpoint(endpoints);

		return endpoints;
	}
}
