using SmartSchool.Modules.Examinations.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Persistence;
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
        services.AddScoped<IExamQuery, ExamQuery>();
        services.AddScoped<IExamCommand, ExamCommand>();
        services.AddScoped<IExamSubjectQuery, ExamSubjectQuery>();
        services.AddScoped<IExamSubjectCommand, ExamSubjectCommand>();
        services.AddScoped<IGradeScaleQuery, GradeScaleQuery>();
        services.AddScoped<IGradeScaleCommand, GradeScaleCommand>();
        services.AddScoped<IStudentExamResultQuery, StudentExamResultQuery>();
        services.AddScoped<IStudentExamResultCommand, StudentExamResultCommand>();
        services.AddScoped<IValidator<CreateExam.Request>, CreateExam.Validator>();
        services.AddScoped<IValidator<UpdateExam.Request>, UpdateExam.Validator>();
        services.AddScoped<IValidator<CreateExamSubject.Request>, CreateExamSubject.Validator>();
        services.AddScoped<IValidator<UpdateExamSubject.Request>, UpdateExamSubject.Validator>();
        services.AddScoped<IValidator<CreateGradeScale.Request>, CreateGradeScale.Validator>();
        services.AddScoped<IValidator<UpdateGradeScale.Request>, UpdateGradeScale.Validator>();
        services.AddScoped<IValidator<CreateStudentExamResult.Request>, CreateStudentExamResult.Validator>();
        services.AddScoped<IValidator<UpdateStudentExamResult.Request>, UpdateStudentExamResult.Validator>();


        services.AddScoped<IRequestHandler<CreateExam.Request, Result<ExamResponse>>, CreateExam.Handler>();
        services.AddScoped<IRequestHandler<GetExamById.Query, Result<ExamResponse>>, GetExamById.Handler>();
        services.AddScoped<IRequestHandler<GetExamPage.Query, Result<PagedResult<ExamResponse>>>, GetExamPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateExam.Request, Result<ExamResponse>>, UpdateExam.Handler>();
        services.AddScoped<IRequestHandler<DeleteExam.Command, Result<DeleteExam.Response>>, DeleteExam.Handler>();
        services.AddScoped<IRequestHandler<CreateExamSubject.Request, Result<ExamSubjectResponse>>, CreateExamSubject.Handler>();
        services.AddScoped<IRequestHandler<GetExamSubjectById.Query, Result<ExamSubjectResponse>>, GetExamSubjectById.Handler>();
        services.AddScoped<IRequestHandler<GetExamSubjectPage.Query, Result<PagedResult<ExamSubjectResponse>>>, GetExamSubjectPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateExamSubject.Request, Result<ExamSubjectResponse>>, UpdateExamSubject.Handler>();
        services.AddScoped<IRequestHandler<DeleteExamSubject.Command, Result<DeleteExamSubject.Response>>, DeleteExamSubject.Handler>();
        services.AddScoped<IRequestHandler<CreateGradeScale.Request, Result<GradeScaleResponse>>, CreateGradeScale.Handler>();
        services.AddScoped<IRequestHandler<GetGradeScaleById.Query, Result<GradeScaleResponse>>, GetGradeScaleById.Handler>();
        services.AddScoped<IRequestHandler<GetGradeScalePage.Query, Result<PagedResult<GradeScaleResponse>>>, GetGradeScalePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateGradeScale.Request, Result<GradeScaleResponse>>, UpdateGradeScale.Handler>();
        services.AddScoped<IRequestHandler<DeleteGradeScale.Command, Result<DeleteGradeScale.Response>>, DeleteGradeScale.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentExamResult.Request, Result<StudentExamResultResponse>>, CreateStudentExamResult.Handler>();
        services.AddScoped<IRequestHandler<GetStudentExamResultById.Query, Result<StudentExamResultResponse>>, GetStudentExamResultById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentExamResultPage.Query, Result<PagedResult<StudentExamResultResponse>>>, GetStudentExamResultPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentExamResult.Request, Result<StudentExamResultResponse>>, UpdateStudentExamResult.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentExamResult.Command, Result<DeleteStudentExamResult.Response>>, DeleteStudentExamResult.Handler>();

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
