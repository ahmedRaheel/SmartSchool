using FluentValidation;
using SmartSchool.Modules.Examinations.Features.Exam;
using SmartSchool.Modules.Examinations.Features.ExamSubject;
using SmartSchool.Modules.Examinations.Features.GradeScale;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;

namespace SmartSchool.Modules.Examinations;

public static class Module
{
    public static IServiceCollection AddExaminationsModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateExam.Handler>();
        services.AddScoped<GetExamById.Handler>();
        services.AddScoped<GetExamPage.Handler>();
        services.AddScoped<UpdateExam.Handler>();
        services.AddScoped<DeleteExam.Handler>();
        services.AddScoped<IValidator<CreateExam.Request>, CreateExam.Validator>();
        services.AddScoped<IValidator<UpdateExam.Request>, UpdateExam.Validator>();
        services.AddScoped<CreateExamSubject.Handler>();
        services.AddScoped<GetExamSubjectById.Handler>();
        services.AddScoped<GetExamSubjectPage.Handler>();
        services.AddScoped<UpdateExamSubject.Handler>();
        services.AddScoped<DeleteExamSubject.Handler>();
        services.AddScoped<IValidator<CreateExamSubject.Request>, CreateExamSubject.Validator>();
        services.AddScoped<IValidator<UpdateExamSubject.Request>, UpdateExamSubject.Validator>();
        services.AddScoped<CreateGradeScale.Handler>();
        services.AddScoped<GetGradeScaleById.Handler>();
        services.AddScoped<GetGradeScalePage.Handler>();
        services.AddScoped<UpdateGradeScale.Handler>();
        services.AddScoped<DeleteGradeScale.Handler>();
        services.AddScoped<IValidator<CreateGradeScale.Request>, CreateGradeScale.Validator>();
        services.AddScoped<IValidator<UpdateGradeScale.Request>, UpdateGradeScale.Validator>();
        services.AddScoped<CreateStudentExamResult.Handler>();
        services.AddScoped<GetStudentExamResultById.Handler>();
        services.AddScoped<GetStudentExamResultPage.Handler>();
        services.AddScoped<UpdateStudentExamResult.Handler>();
        services.AddScoped<DeleteStudentExamResult.Handler>();
        services.AddScoped<IValidator<CreateStudentExamResult.Request>, CreateStudentExamResult.Validator>();
        services.AddScoped<IValidator<UpdateStudentExamResult.Request>, UpdateStudentExamResult.Validator>();

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
