using SmartSchool.Modules.Organization.Features.AcademicSystem;
using SmartSchool.Modules.Organization.Features.AcademicYear;
using SmartSchool.Modules.Organization.Features.ClassSection;
using SmartSchool.Modules.Organization.Features.CourseOffering;
using SmartSchool.Modules.Organization.Features.GradeLevel;
using SmartSchool.Modules.Organization.Features.Program;
using SmartSchool.Modules.Organization.Features.Subject;
using SmartSchool.Modules.Organization.Features.Term;
using SmartSchool.Modules.Organization.Features.Timetable;
using SmartSchool.Modules.Organization.Persistence;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Modules.Organization.Features.Campus;
using SmartSchool.Modules.Organization.Features.Department;
using SmartSchool.Modules.Organization.Features.School;
using SmartSchool.Application.Messaging;
using SmartSchool.Application;
using SmartSchool.Modules.Organization.Features.Organization;




public static class Module
{
	public static IServiceCollection AddOrganizationModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IOrganizationDbContext, OrganizationDbContext>();

        services.AddFeaturePersistence(typeof(Module).Assembly);
		services.AddScoped<ICampusCommand, CampusCommand>();
		services.AddScoped<ICampusQuery, CampusQuery>();
		services.AddScoped<IDepartmentCommand, DepartmentCommand>();
		services.AddScoped<IDepartmentQuery, DepartmentQuery>();
		services.AddScoped<ISchoolCommand, SchoolCommand>();
		services.AddScoped<ISchoolQuery, SchoolQuery>();
		services.AddScoped<IBranchPolicyCommand, BranchPolicyCommand>();
		services.AddScoped<IBranchPolicyQuery, BranchPolicyQuery>();
		
		return services;
	}

	public static IEndpointRouteBuilder MapOrganizationEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		// Academics is campus-owned and therefore implemented by Organization.
		// Public /api/academics routes are retained for UI/API compatibility.
		CreateAcademicSystem.MapEndpoint(endpoints);
		GetAcademicSystemById.MapEndpoint(endpoints);
		GetAcademicSystemPage.MapEndpoint(endpoints);
		UpdateAcademicSystem.MapEndpoint(endpoints);
		DeleteAcademicSystem.MapEndpoint(endpoints);
		CreateAcademicYear.MapEndpoint(endpoints);
		GetAcademicYearById.MapEndpoint(endpoints);
		GetAcademicYearPage.MapEndpoint(endpoints);
		UpdateAcademicYear.MapEndpoint(endpoints);
		DeleteAcademicYear.MapEndpoint(endpoints);
		CreateClassSection.MapEndpoint(endpoints);
		GetClassSectionById.MapEndpoint(endpoints);
		GetClassSectionPage.MapEndpoint(endpoints);
		UpdateClassSection.MapEndpoint(endpoints);
		DeleteClassSection.MapEndpoint(endpoints);
		CreateCourseOffering.MapEndpoint(endpoints);
		GetCourseOfferingById.MapEndpoint(endpoints);
		GetCourseOfferingPage.MapEndpoint(endpoints);
		UpdateCourseOffering.MapEndpoint(endpoints);
		DeleteCourseOffering.MapEndpoint(endpoints);
		CreateGradeLevel.MapEndpoint(endpoints);
		GetGradeLevelById.MapEndpoint(endpoints);
		GetGradeLevelPage.MapEndpoint(endpoints);
		UpdateGradeLevel.MapEndpoint(endpoints);
		DeleteGradeLevel.MapEndpoint(endpoints);
		CreateProgram.MapEndpoint(endpoints);
		GetProgramById.MapEndpoint(endpoints);
		GetProgramPage.MapEndpoint(endpoints);
		UpdateProgram.MapEndpoint(endpoints);
		DeleteProgram.MapEndpoint(endpoints);
		CreateSubject.MapEndpoint(endpoints);
		GetSubjectById.MapEndpoint(endpoints);
		GetSubjectPage.MapEndpoint(endpoints);
		UpdateSubject.MapEndpoint(endpoints);
		DeleteSubject.MapEndpoint(endpoints);
		CreateTerm.MapEndpoint(endpoints);
		GetTermById.MapEndpoint(endpoints);
		GetTermPage.MapEndpoint(endpoints);
		UpdateTerm.MapEndpoint(endpoints);
		DeleteTerm.MapEndpoint(endpoints);
		CreateTimetable.MapEndpoint(endpoints);
		GetTimetableById.MapEndpoint(endpoints);
		GetTimetablePage.MapEndpoint(endpoints);
		UpdateTimetable.MapEndpoint(endpoints);
		DeleteTimetable.MapEndpoint(endpoints);

		CreateTenant.MapEndpoint(endpoints);
		GetOrganizationById.MapEndpoint(endpoints);
		GetOrganizationPage.MapEndpoint(endpoints);
		UpdateOrganization.MapEndpoint(endpoints);
		DeleteOrganization.MapEndpoint(endpoints);
		CreateCampus.MapEndpoint(endpoints);
		BranchPolicyEndpoints.MapEndpoints(endpoints);
		GetCampusById.MapEndpoint(endpoints);
		GetCampusPage.MapEndpoint(endpoints);
		UpdateCampus.MapEndpoint(endpoints);
		DeleteCampus.MapEndpoint(endpoints);
		CreateDepartment.MapEndpoint(endpoints);
		GetDepartmentById.MapEndpoint(endpoints);
		GetDepartmentPage.MapEndpoint(endpoints);
		UpdateDepartment.MapEndpoint(endpoints);
		DeleteDepartment.MapEndpoint(endpoints);

		CreateSchool.MapEndpoint(endpoints);
		DeleteSchool.MapEndpoint(endpoints);
		GetSchoolById.MapEndpoint(endpoints);
		GetSchoolPage.MapEndpoint(endpoints);
		UpdateSchool.MapEndpoint(endpoints);

		return endpoints;
	}
}

