using SmartSchool.Modules.AIPrediction.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;
using SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;
using SmartSchool.Modules.AIPrediction.Features.ExamPrediction;
using SmartSchool.Modules.AIPrediction.Features.PredictionSuite;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;
using SmartSchool.Modules.AIPrediction.Features.PredictionModel;
using SmartSchool.Modules.AIPrediction.Features.StudentIntervention;
using SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;
using SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;
using SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction;

public static class Module
{
    public static IServiceCollection AddAIPredictionModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IAIPredictionDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<AIPredictionDbContext>());

        services.AddFeaturePersistence(typeof(Module).Assembly);
        services.AddScoped<IExamPredictionService, MlNetExamPredictionService>();
        services.AddScoped<IPredictionSuiteService, MlNetPredictionSuiteService>();

        return services;
    }

    public static IEndpointRouteBuilder MapAIPredictionEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateClassPerformanceInsight.MapEndpoint(endpoints);
        PredictExamPerformance.MapEndpoint(endpoints);
        PredictStudent.MapEndpoint(endpoints);
        GetEarlyWarning.MapEndpoint(endpoints);
        PredictAdmission.MapEndpoint(endpoints);
        PredictTeacher.MapEndpoint(endpoints);
        PredictPayrollAnomaly.MapEndpoint(endpoints);
        PredictTransportDelay.MapEndpoint(endpoints);
        PredictLibraryOverdue.MapEndpoint(endpoints);
        ForecastPrediction.MapEndpoint(endpoints);
        GetClassPerformanceInsightById.MapEndpoint(endpoints);
        GetClassPerformanceInsightByTermId.MapEndpoint(endpoints);
        GetClassPerformanceInsightByTeacherEmployeeId.MapEndpoint(endpoints);
        GetClassPerformanceInsightByCourseOfferingId.MapEndpoint(endpoints);
        GetClassPerformanceInsightByClassSectionId.MapEndpoint(endpoints);
        GetClassPerformanceInsightByAcademicYearId.MapEndpoint(endpoints);
        GetClassPerformanceInsightPage.MapEndpoint(endpoints);
        UpdateClassPerformanceInsight.MapEndpoint(endpoints);
        DeleteClassPerformanceInsight.MapEndpoint(endpoints);
        CreatePredictionEvaluation.MapEndpoint(endpoints);
        GetPredictionEvaluationById.MapEndpoint(endpoints);
        GetPredictionEvaluationByStudentPerformancePredictionId.MapEndpoint(endpoints);
        GetPredictionEvaluationByStudentExamResultId.MapEndpoint(endpoints);
        GetPredictionEvaluationPage.MapEndpoint(endpoints);
        UpdatePredictionEvaluation.MapEndpoint(endpoints);
        DeletePredictionEvaluation.MapEndpoint(endpoints);
        CreatePredictionEvidence.MapEndpoint(endpoints);
        GetPredictionEvidenceById.MapEndpoint(endpoints);
        GetPredictionEvidenceByStudentPerformancePredictionId.MapEndpoint(endpoints);
        GetPredictionEvidencePage.MapEndpoint(endpoints);
        UpdatePredictionEvidence.MapEndpoint(endpoints);
        DeletePredictionEvidence.MapEndpoint(endpoints);
        CreatePredictionModel.MapEndpoint(endpoints);
        GetPredictionModelById.MapEndpoint(endpoints);
        GetPredictionModelPage.MapEndpoint(endpoints);
        UpdatePredictionModel.MapEndpoint(endpoints);
        DeletePredictionModel.MapEndpoint(endpoints);
        CreateStudentIntervention.MapEndpoint(endpoints);
        GetStudentInterventionById.MapEndpoint(endpoints);
        GetStudentInterventionByTeacherEmployeeId.MapEndpoint(endpoints);
        GetStudentInterventionBySubjectId.MapEndpoint(endpoints);
        GetStudentInterventionByStudentId.MapEndpoint(endpoints);
        GetStudentInterventionBySourceRecommendationId.MapEndpoint(endpoints);
        GetStudentInterventionBySourcePredictionId.MapEndpoint(endpoints);
        GetStudentInterventionByCourseOfferingId.MapEndpoint(endpoints);
        GetStudentInterventionPage.MapEndpoint(endpoints);
        UpdateStudentIntervention.MapEndpoint(endpoints);
        DeleteStudentIntervention.MapEndpoint(endpoints);
        CreateStudentPerformancePrediction.MapEndpoint(endpoints);
        GetStudentPerformancePredictionById.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByTermId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByTargetExamSubjectId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByTargetExamId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionBySubjectId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByStudentId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByPredictionModelId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByCourseOfferingId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionByAcademicYearId.MapEndpoint(endpoints);
        GetStudentPerformancePredictionPage.MapEndpoint(endpoints);
        UpdateStudentPerformancePrediction.MapEndpoint(endpoints);
        DeleteStudentPerformancePrediction.MapEndpoint(endpoints);
        CreateTeachingRecommendation.MapEndpoint(endpoints);
        GetTeachingRecommendationById.MapEndpoint(endpoints);
        GetTeachingRecommendationByTeacherEmployeeId.MapEndpoint(endpoints);
        GetTeachingRecommendationBySubjectId.MapEndpoint(endpoints);
        GetTeachingRecommendationByCourseOfferingId.MapEndpoint(endpoints);
        GetTeachingRecommendationByClassSectionId.MapEndpoint(endpoints);
        GetTeachingRecommendationByClassPerformanceInsightId.MapEndpoint(endpoints);
        GetTeachingRecommendationPage.MapEndpoint(endpoints);
        UpdateTeachingRecommendation.MapEndpoint(endpoints);
        DeleteTeachingRecommendation.MapEndpoint(endpoints);
        CreateTopicPerformanceInsight.MapEndpoint(endpoints);
        GetTopicPerformanceInsightById.MapEndpoint(endpoints);
        GetTopicPerformanceInsightBySubjectId.MapEndpoint(endpoints);
        GetTopicPerformanceInsightByClassPerformanceInsightId.MapEndpoint(endpoints);
        GetTopicPerformanceInsightPage.MapEndpoint(endpoints);
        UpdateTopicPerformanceInsight.MapEndpoint(endpoints);
        DeleteTopicPerformanceInsight.MapEndpoint(endpoints);

        return endpoints;
    }
}
