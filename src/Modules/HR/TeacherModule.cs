using SmartSchool.Modules.HR.Features.Teacher.ApplyTeacherLeave;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacher;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherAssignments;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherClasses;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherDashboard;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherProfile;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherStudents;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherTimetable;
using SmartSchool.Modules.HR.Features.Teacher.GetTeacherWorkload;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR;

public static class TeacherModule
{
    public static IEndpointRouteBuilder MapTeachersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/teachers")
            .WithTags("Teachers")
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);

        GetTeacherProfile.MapEndpoint(group);
        GetTeacher.MapEndpoint(group);
        GetTeacherClasses.MapEndpoint(group);
        GetTeacherStudents.MapEndpoint(group);
        GetTeacherTimetable.MapEndpoint(group);
        GetTeacherAssignments.MapEndpoint(group);
        GetTeacherWorkload.MapEndpoint(group);
        GetTeacherDashboard.MapEndpoint(group);
        ApplyTeacherLeave.MapEndpoint(group);

        return endpoints;
    }
}
