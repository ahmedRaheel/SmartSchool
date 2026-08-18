
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Features.Exam;
using SmartSchool.Modules.Examinations.Features.ExamSubject;
using SmartSchool.Modules.Examinations.Features.GradeScale;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations;

public static class Module
{
    public static IServiceCollection AddExaminationsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IExamQuery, ExamQuery>();
        services.AddScoped<IExamCommand, ExamCommand>();
        services.AddScoped<IExamSubjectQuery, ExamSubjectQuery>();
        services.AddScoped<IExamSubjectCommand, ExamSubjectCommand>();
        services.AddScoped<IGradeScaleQuery, GradeScaleQuery>();
        services.AddScoped<IGradeScaleCommand, GradeScaleCommand>();
        services.AddScoped<IStudentExamResultQuery, StudentExamResultQuery>();
        services.AddScoped<IStudentExamResultCommand, StudentExamResultCommand>();

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
        CreateGradeScale.MapEndpoint(endpoints);
        GetGradeScaleById.MapEndpoint(endpoints);
        GetGradeScalePage.MapEndpoint(endpoints);
        UpdateGradeScale.MapEndpoint(endpoints);
        DeleteGradeScale.MapEndpoint(endpoints);
        CreateStudentExamResult.MapEndpoint(endpoints);
        GetStudentExamResultById.MapEndpoint(endpoints);
        GetStudentExamResultPage.MapEndpoint(endpoints);
        UpdateStudentExamResult.MapEndpoint(endpoints);
        DeleteStudentExamResult.MapEndpoint(endpoints);

        return endpoints;
    }
}
