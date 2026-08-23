using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Features.Enrollment;
using SmartSchool.Modules.Students.Features.Guardian;
using SmartSchool.Modules.Students.Features.Student;
using SmartSchool.Modules.Students.Features.StudentGuardian;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students;

public static class Module
{
	public static IServiceCollection AddStudentsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IEnrollmentQuery, EnrollmentQuery>();
		services.AddScoped<IEnrollmentCommand, EnrollmentCommand>();
		services.AddScoped<IGuardianQuery, GuardianQuery>();
		services.AddScoped<IGuardianCommand, GuardianCommand>();
		services.AddScoped<IStudentQuery, StudentQuery>();
		services.AddScoped<IStudentCommand, StudentCommand>();
		services.AddScoped<IStudentGuardianQuery, StudentGuardianQuery>();
		services.AddScoped<IStudentGuardianCommand, StudentGuardianCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapStudentsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateEnrollment.MapEndpoint(endpoints);
		GetEnrollmentById.MapEndpoint(endpoints);
		GetEnrollmentPage.MapEndpoint(endpoints);
		UpdateEnrollment.MapEndpoint(endpoints);
		DeleteEnrollment.MapEndpoint(endpoints);
		CreateGuardian.MapEndpoint(endpoints);
		GetGuardianById.MapEndpoint(endpoints);
		GetGuardianPage.MapEndpoint(endpoints);
		UpdateGuardian.MapEndpoint(endpoints);
		DeleteGuardian.MapEndpoint(endpoints);
		CreateStudent.MapEndpoint(endpoints);
		GetStudentById.MapEndpoint(endpoints);
		GetStudentPage.MapEndpoint(endpoints);
		UpdateStudent.MapEndpoint(endpoints);
		DeleteStudent.MapEndpoint(endpoints);
		CreateStudentGuardian.MapEndpoint(endpoints);
		GetStudentGuardianById.MapEndpoint(endpoints);
		GetStudentGuardianPage.MapEndpoint(endpoints);
		UpdateStudentGuardian.MapEndpoint(endpoints);
		DeleteStudentGuardian.MapEndpoint(endpoints);

		return endpoints;
	}
}
