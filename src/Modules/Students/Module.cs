using SmartSchool.Modules.Students.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateAttendance.Request>, CreateAttendance.Validator>();
        services.AddScoped<IValidator<UpdateAttendance.Request>, UpdateAttendance.Validator>();
        services.AddScoped<IValidator<CreateEnrollment.Request>, CreateEnrollment.Validator>();
        services.AddScoped<IValidator<UpdateEnrollment.Request>, UpdateEnrollment.Validator>();
        services.AddScoped<IValidator<CreateGuardian.Request>, CreateGuardian.Validator>();
        services.AddScoped<IValidator<UpdateGuardian.Request>, UpdateGuardian.Validator>();
        services.AddScoped<IValidator<CreateStudent.Request>, CreateStudent.Validator>();
        services.AddScoped<IValidator<UpdateStudent.Request>, UpdateStudent.Validator>();
        services.AddScoped<IValidator<CreateStudentGuardian.Request>, CreateStudentGuardian.Validator>();
        services.AddScoped<IValidator<UpdateStudentGuardian.Request>, UpdateStudentGuardian.Validator>();


        services.AddScoped<IRequestHandler<CreateAttendance.Request, Result<AttendanceResponse>>, CreateAttendance.Handler>();
        services.AddScoped<IRequestHandler<GetAttendanceById.Query, Result<AttendanceResponse>>, GetAttendanceById.Handler>();
        services.AddScoped<IRequestHandler<GetAttendancePage.Query, Result<PagedResult<AttendanceResponse>>>, GetAttendancePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateAttendance.Request, Result<AttendanceResponse>>, UpdateAttendance.Handler>();
        services.AddScoped<IRequestHandler<DeleteAttendance.Command, Result<DeleteAttendance.Response>>, DeleteAttendance.Handler>();
        services.AddScoped<IRequestHandler<CreateEnrollment.Request, Result<EnrollmentResponse>>, CreateEnrollment.Handler>();
        services.AddScoped<IRequestHandler<GetEnrollmentById.Query, Result<EnrollmentResponse>>, GetEnrollmentById.Handler>();
        services.AddScoped<IRequestHandler<GetEnrollmentPage.Query, Result<PagedResult<EnrollmentResponse>>>, GetEnrollmentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateEnrollment.Request, Result<EnrollmentResponse>>, UpdateEnrollment.Handler>();
        services.AddScoped<IRequestHandler<DeleteEnrollment.Command, Result<DeleteEnrollment.Response>>, DeleteEnrollment.Handler>();
        services.AddScoped<IRequestHandler<CreateGuardian.Request, Result<GuardianResponse>>, CreateGuardian.Handler>();
        services.AddScoped<IRequestHandler<GetGuardianById.Query, Result<GuardianResponse>>, GetGuardianById.Handler>();
        services.AddScoped<IRequestHandler<GetGuardianPage.Query, Result<PagedResult<GuardianResponse>>>, GetGuardianPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateGuardian.Request, Result<GuardianResponse>>, UpdateGuardian.Handler>();
        services.AddScoped<IRequestHandler<DeleteGuardian.Command, Result<DeleteGuardian.Response>>, DeleteGuardian.Handler>();
        services.AddScoped<IRequestHandler<CreateStudent.Request, Result<StudentResponse>>, CreateStudent.Handler>();
        services.AddScoped<IRequestHandler<GetStudentById.Query, Result<StudentResponse>>, GetStudentById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentPage.Query, Result<PagedResult<StudentResponse>>>, GetStudentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudent.Request, Result<StudentResponse>>, UpdateStudent.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudent.Command, Result<DeleteStudent.Response>>, DeleteStudent.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentGuardian.Request, Result<StudentGuardianResponse>>, CreateStudentGuardian.Handler>();
        services.AddScoped<IRequestHandler<GetStudentGuardianById.Query, Result<StudentGuardianResponse>>, GetStudentGuardianById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentGuardianPage.Query, Result<PagedResult<StudentGuardianResponse>>>, GetStudentGuardianPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentGuardian.Request, Result<StudentGuardianResponse>>, UpdateStudentGuardian.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentGuardian.Command, Result<DeleteStudentGuardian.Response>>, DeleteStudentGuardian.Handler>();

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
