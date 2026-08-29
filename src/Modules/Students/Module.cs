using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Features.Attendance;
using SmartSchool.Modules.Students.Features.Enrollment;
using SmartSchool.Modules.Students.Features.Guardian;
using SmartSchool.Modules.Students.Features.Student;
using SmartSchool.Modules.Students.Features.StudentGuardian;

using SmartSchool.Modules.Students.Features.DataAccess.StudentOnboarding;

namespace SmartSchool.Modules.Students;

/// <summary>
/// Registers the Students bounded context and exposes its HTTP endpoints.
/// </summary>
public static class Module
{
    /// <summary>
    /// Registers Students application handlers and persistence services.
    /// </summary>
    public static IServiceCollection AddStudentsModule(this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeatureDataAccess(typeof(Module).Assembly);
        return services;
    }

    /// <summary>
    /// Maps all HTTP endpoints owned by the Students bounded context.
    /// </summary>
    public static IEndpointRouteBuilder MapStudentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        MapStudentEndpoints(endpoints);
        MapGuardianEndpoints(endpoints);
        MapEnrollmentEndpoints(endpoints);
        MapAttendanceEndpoints(endpoints);

        return endpoints;
    }

    private static void MapStudentEndpoints(IEndpointRouteBuilder endpoints)
    {
        CreateStudent.MapEndpoint(endpoints);
        GetStudentById.MapEndpoint(endpoints);
        GetStudentPage.MapEndpoint(endpoints);
        UpdateStudent.MapEndpoint(endpoints);
        DeleteStudent.MapEndpoint(endpoints);
        ApproveStudentAdmission.MapEndpoint(endpoints);
        StrikeOffStudent.MapEndpoint(endpoints);

        CreateStudentGuardian.MapEndpoint(endpoints);
        LinkStudentGuardian.MapEndpoint(endpoints);
        GetStudentGuardianById.MapEndpoint(endpoints);
        GetStudentGuardianPage.MapEndpoint(endpoints);
        UpdateStudentGuardian.MapEndpoint(endpoints);
        DeleteStudentGuardian.MapEndpoint(endpoints);
    }

    private static void MapGuardianEndpoints(IEndpointRouteBuilder endpoints)
    {
        CreateGuardian.MapEndpoint(endpoints);
        GetGuardianById.MapEndpoint(endpoints);
        GetGuardianPage.MapEndpoint(endpoints);
        UpdateGuardian.MapEndpoint(endpoints);
        DeleteGuardian.MapEndpoint(endpoints);
    }

    private static void MapEnrollmentEndpoints(IEndpointRouteBuilder endpoints)
    {
        CreateEnrollment.MapEndpoint(endpoints);
        GetEnrollmentById.MapEndpoint(endpoints);
        GetEnrollmentPage.MapEndpoint(endpoints);
        UpdateEnrollment.MapEndpoint(endpoints);
        DeleteEnrollment.MapEndpoint(endpoints);
    }

    private static void MapAttendanceEndpoints(IEndpointRouteBuilder endpoints)
    {
        CreateAttendance.MapEndpoint(endpoints);
        GetAttendanceById.MapEndpoint(endpoints);
        GetAttendancePage.MapEndpoint(endpoints);
        UpdateAttendance.MapEndpoint(endpoints);
        DeleteAttendance.MapEndpoint(endpoints);
    }
}
