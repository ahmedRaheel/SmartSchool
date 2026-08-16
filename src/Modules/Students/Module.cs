using SmartSchool.Modules.Students.Persistence;
using FluentValidation;
using SmartSchool.Modules.Students.Features.Attendance;
using SmartSchool.Modules.Students.Features.Enrollment;
using SmartSchool.Modules.Students.Features.Guardian;
using SmartSchool.Modules.Students.Features.Student;
using SmartSchool.Modules.Students.Features.StudentGuardian;

namespace SmartSchool.Modules.Students;

public static class Module
{
    public static IServiceCollection AddStudentsModule(
        this IServiceCollection services)
    {
        services.AddScoped<IAttendanceQuery, AttendanceQuery>();
        services.AddScoped<IAttendanceCommand, AttendanceCommand>();
        services.AddScoped<IEnrollmentQuery, EnrollmentQuery>();
        services.AddScoped<IEnrollmentCommand, EnrollmentCommand>();
        services.AddScoped<IGuardianQuery, GuardianQuery>();
        services.AddScoped<IGuardianCommand, GuardianCommand>();
        services.AddScoped<IStudentQuery, StudentQuery>();
        services.AddScoped<IStudentCommand, StudentCommand>();
        services.AddScoped<IStudentGuardianQuery, StudentGuardianQuery>();
        services.AddScoped<IStudentGuardianCommand, StudentGuardianCommand>();

        services.AddScoped<CreateAttendance.Handler>();
        services.AddScoped<GetAttendanceById.Handler>();
        services.AddScoped<GetAttendancePage.Handler>();
        services.AddScoped<UpdateAttendance.Handler>();
        services.AddScoped<DeleteAttendance.Handler>();
        services.AddScoped<IValidator<CreateAttendance.Request>, CreateAttendance.Validator>();
        services.AddScoped<IValidator<UpdateAttendance.Request>, UpdateAttendance.Validator>();
        services.AddScoped<CreateEnrollment.Handler>();
        services.AddScoped<GetEnrollmentById.Handler>();
        services.AddScoped<GetEnrollmentPage.Handler>();
        services.AddScoped<UpdateEnrollment.Handler>();
        services.AddScoped<DeleteEnrollment.Handler>();
        services.AddScoped<IValidator<CreateEnrollment.Request>, CreateEnrollment.Validator>();
        services.AddScoped<IValidator<UpdateEnrollment.Request>, UpdateEnrollment.Validator>();
        services.AddScoped<CreateGuardian.Handler>();
        services.AddScoped<GetGuardianById.Handler>();
        services.AddScoped<GetGuardianPage.Handler>();
        services.AddScoped<UpdateGuardian.Handler>();
        services.AddScoped<DeleteGuardian.Handler>();
        services.AddScoped<IValidator<CreateGuardian.Request>, CreateGuardian.Validator>();
        services.AddScoped<IValidator<UpdateGuardian.Request>, UpdateGuardian.Validator>();
        services.AddScoped<CreateStudent.Handler>();
        services.AddScoped<GetStudentById.Handler>();
        services.AddScoped<GetStudentPage.Handler>();
        services.AddScoped<UpdateStudent.Handler>();
        services.AddScoped<DeleteStudent.Handler>();
        services.AddScoped<IValidator<CreateStudent.Request>, CreateStudent.Validator>();
        services.AddScoped<IValidator<UpdateStudent.Request>, UpdateStudent.Validator>();
        services.AddScoped<CreateStudentGuardian.Handler>();
        services.AddScoped<GetStudentGuardianById.Handler>();
        services.AddScoped<GetStudentGuardianPage.Handler>();
        services.AddScoped<UpdateStudentGuardian.Handler>();
        services.AddScoped<DeleteStudentGuardian.Handler>();
        services.AddScoped<IValidator<CreateStudentGuardian.Request>, CreateStudentGuardian.Validator>();
        services.AddScoped<IValidator<UpdateStudentGuardian.Request>, UpdateStudentGuardian.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapStudentsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateAttendance.MapEndpoint(endpoints);
        GetAttendanceById.MapEndpoint(endpoints);
        GetAttendancePage.MapEndpoint(endpoints);
        UpdateAttendance.MapEndpoint(endpoints);
        DeleteAttendance.MapEndpoint(endpoints);
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
