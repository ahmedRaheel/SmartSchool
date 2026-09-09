using SmartSchool.Modules.Organization.Features.TenantSettings;
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
        services.AddScoped<IOrganizationDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<OrganizationDbContext>());

        services.AddFeaturePersistence(typeof(Module).Assembly);
        services.AddScoped<CreateCampusBranchPolicyCommand>();
        services.AddScoped<CreateCampusCampusCommand>();
        services.AddScoped<CreateCampusSchoolQuery>();
        services.AddScoped<UpdateCampusBranchPolicyCommand>();
        services.AddScoped<UpdateCampusCampusCommand>();
        services.AddScoped<UpdateCampusSchoolQuery>();
        services.AddScoped<UpdateSchoolSchoolQuery>();
        services.AddScoped<UpdateSchoolSchoolCommand>();
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
        GetAcademicYearsByCampusId.MapEndpoint(endpoints);
        UpdateAcademicYear.MapEndpoint(endpoints);
        DeleteAcademicYear.MapEndpoint(endpoints);
        CreateClassSection.MapEndpoint(endpoints);
        GetClassSectionById.MapEndpoint(endpoints);
        GetClassSectionByClassTeacherEmployeeId.MapEndpoint(endpoints);
        GetClassSectionByRoomId.MapEndpoint(endpoints);
        GetClassSectionByProgramGradeId.MapEndpoint(endpoints);
        GetClassSectionByCampusId.MapEndpoint(endpoints);
        GetClassSectionByAcademicYearId.MapEndpoint(endpoints);
        GetClassSectionPage.MapEndpoint(endpoints);
        GetClassSectionsByParentId.MapEndpoint(endpoints);
        UpdateClassSection.MapEndpoint(endpoints);
        DeleteClassSection.MapEndpoint(endpoints);
        CreateCourseOffering.MapEndpoint(endpoints);
        GetCourseOfferingById.MapEndpoint(endpoints);
        GetCourseOfferingByTermId.MapEndpoint(endpoints);
        GetCourseOfferingByProgramSubjectId.MapEndpoint(endpoints);
        GetCourseOfferingByCampusId.MapEndpoint(endpoints);
        GetCourseOfferingByAcademicYearId.MapEndpoint(endpoints);
        GetCourseOfferingPage.MapEndpoint(endpoints);
        UpdateCourseOffering.MapEndpoint(endpoints);
        DeleteCourseOffering.MapEndpoint(endpoints);
        CreateGradeLevel.MapEndpoint(endpoints);
        GetGradeLevelById.MapEndpoint(endpoints);
        GetGradeLevelPage.MapEndpoint(endpoints);
        GetGradeLevelsByCampusId.MapEndpoint(endpoints);
        UpdateGradeLevel.MapEndpoint(endpoints);
        DeleteGradeLevel.MapEndpoint(endpoints);
        CreateProgram.MapEndpoint(endpoints);
        GetProgramById.MapEndpoint(endpoints);
        GetProgramByAcademicSystemId.MapEndpoint(endpoints);
        GetProgramPage.MapEndpoint(endpoints);
        UpdateProgram.MapEndpoint(endpoints);
        DeleteProgram.MapEndpoint(endpoints);
        CreateSubject.MapEndpoint(endpoints);
        GetSubjectById.MapEndpoint(endpoints);
        GetSubjectByDepartmentId.MapEndpoint(endpoints);
        GetSubjectPage.MapEndpoint(endpoints);
        UpdateSubject.MapEndpoint(endpoints);
        DeleteSubject.MapEndpoint(endpoints);
        CreateTerm.MapEndpoint(endpoints);
        GetTermById.MapEndpoint(endpoints);
        GetTermByAcademicYearId.MapEndpoint(endpoints);
        GetTermPage.MapEndpoint(endpoints);
        UpdateTerm.MapEndpoint(endpoints);
        DeleteTerm.MapEndpoint(endpoints);
        CreateTimetable.MapEndpoint(endpoints);
        GetTimetableById.MapEndpoint(endpoints);
        GetTimetableByTermId.MapEndpoint(endpoints);
        GetTimetableByCampusId.MapEndpoint(endpoints);
        GetTimetableByAcademicYearId.MapEndpoint(endpoints);
        GetTimetablePage.MapEndpoint(endpoints);
        UpdateTimetable.MapEndpoint(endpoints);
        DeleteTimetable.MapEndpoint(endpoints);

        GetTenantSettings.MapEndpoint(endpoints);
        SaveTenantSettings.MapEndpoint(endpoints);

        CreateTenant.MapEndpoint(endpoints);
        GetOrganizationById.MapEndpoint(endpoints);
        GetOrganizationPage.MapEndpoint(endpoints);
        UpdateOrganization.MapEndpoint(endpoints);
        DeleteOrganization.MapEndpoint(endpoints);
        CreateCampus.MapEndpoint(endpoints);
        GetBranchGenderTypes.MapEndpoint(endpoints);
        GetEducationLevels.MapEndpoint(endpoints);
        GetBranchPolicy.MapEndpoint(endpoints);
        GetCampusById.MapEndpoint(endpoints);
        GetCampusPage.MapEndpoint(endpoints);
        GetCurrentCampus.MapEndpoint(endpoints);
        UpdateCampus.MapEndpoint(endpoints);
        DeleteCampus.MapEndpoint(endpoints);
        CreateDepartment.MapEndpoint(endpoints);
        GetDepartmentById.MapEndpoint(endpoints);
        GetDepartmentByCampusId.MapEndpoint(endpoints);
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
