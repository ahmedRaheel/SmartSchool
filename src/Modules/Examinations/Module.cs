using SmartSchool.Modules.Examinations.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Examinations.Features.Exam;
using SmartSchool.Modules.Examinations.Features.ExamSubject;
using SmartSchool.Modules.Examinations.Features.ExamTask;
using SmartSchool.Modules.Examinations.Features.StudentExamResult;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Examinations.Features.GradeScale;
namespace SmartSchool.Modules.Examinations;

public static class Module
{
    public static IServiceCollection AddExaminationsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddModuleDbContext<ExaminationsDbContext, IExaminationsDbContext>(
            configuration,
            ModuleConstants.Schema);

        services.AddFeaturePersistence(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapExaminationsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateExam.MapEndpoint(endpoints);
        ExamTaskWorkflow.MapEndpoints(endpoints);
        GetExamSetup.MapEndpoint(endpoints);
        GetExamResults.MapEndpoint(endpoints);
        SaveExamResults.MapEndpoint(endpoints);
        PublishExamResults.MapEndpoint(endpoints);
        GetExamById.MapEndpoint(endpoints);
        GetExamByTermId.MapEndpoint(endpoints);
        GetExamByCampusId.MapEndpoint(endpoints);
        GetExamByAcademicYearId.MapEndpoint(endpoints);
        GetExamByAcademicSystemId.MapEndpoint(endpoints);
        GetExamPage.MapEndpoint(endpoints);
        DeleteExam.MapEndpoint(endpoints);
        GetExamSubjectById.MapEndpoint(endpoints);
        GetExamSubjectByRoomId.MapEndpoint(endpoints);
        GetExamSubjectByExamId.MapEndpoint(endpoints);
        GetExamSubjectByCourseOfferingId.MapEndpoint(endpoints);
        GetExamSubjectPage.MapEndpoint(endpoints);
        GetStudentExamResultById.MapEndpoint(endpoints);
        GetStudentExamResultByStudentId.MapEndpoint(endpoints);
        GetStudentExamResultByExamSubjectId.MapEndpoint(endpoints);
        GetStudentExamResultPage.MapEndpoint(endpoints);

        CreateGradeScale.MapEndpoint(endpoints);
        DeleteGradeScale.MapEndpoint(endpoints);
        GetGradeScaleById.MapEndpoint(endpoints);
        GetGradeScalePage.MapEndpoint(endpoints);
        UpdateGradeScale.MapEndpoint(endpoints);

        return endpoints;
    }
}
