using FluentValidation;
using SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;
using SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;
using SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;
using SmartSchool.Modules.AIPrediction.Features.PredictionModel;
using SmartSchool.Modules.AIPrediction.Features.StudentIntervention;
using SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;
using SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;
using SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

namespace SmartSchool.Modules.AIPrediction;

public static class Module
{
    public static IServiceCollection AddAIPredictionModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateClassPerformanceInsight.Handler>();
        services.AddScoped<GetClassPerformanceInsightById.Handler>();
        services.AddScoped<GetClassPerformanceInsightPage.Handler>();
        services.AddScoped<UpdateClassPerformanceInsight.Handler>();
        services.AddScoped<DeleteClassPerformanceInsight.Handler>();
        services.AddScoped<IValidator<CreateClassPerformanceInsight.Request>, CreateClassPerformanceInsight.Validator>();
        services.AddScoped<IValidator<UpdateClassPerformanceInsight.Request>, UpdateClassPerformanceInsight.Validator>();
        services.AddScoped<CreatePredictionEvaluation.Handler>();
        services.AddScoped<GetPredictionEvaluationById.Handler>();
        services.AddScoped<GetPredictionEvaluationPage.Handler>();
        services.AddScoped<UpdatePredictionEvaluation.Handler>();
        services.AddScoped<DeletePredictionEvaluation.Handler>();
        services.AddScoped<IValidator<CreatePredictionEvaluation.Request>, CreatePredictionEvaluation.Validator>();
        services.AddScoped<IValidator<UpdatePredictionEvaluation.Request>, UpdatePredictionEvaluation.Validator>();
        services.AddScoped<CreatePredictionEvidence.Handler>();
        services.AddScoped<GetPredictionEvidenceById.Handler>();
        services.AddScoped<GetPredictionEvidencePage.Handler>();
        services.AddScoped<UpdatePredictionEvidence.Handler>();
        services.AddScoped<DeletePredictionEvidence.Handler>();
        services.AddScoped<IValidator<CreatePredictionEvidence.Request>, CreatePredictionEvidence.Validator>();
        services.AddScoped<IValidator<UpdatePredictionEvidence.Request>, UpdatePredictionEvidence.Validator>();
        services.AddScoped<CreatePredictionModel.Handler>();
        services.AddScoped<GetPredictionModelById.Handler>();
        services.AddScoped<GetPredictionModelPage.Handler>();
        services.AddScoped<UpdatePredictionModel.Handler>();
        services.AddScoped<DeletePredictionModel.Handler>();
        services.AddScoped<IValidator<CreatePredictionModel.Request>, CreatePredictionModel.Validator>();
        services.AddScoped<IValidator<UpdatePredictionModel.Request>, UpdatePredictionModel.Validator>();
        services.AddScoped<CreateStudentIntervention.Handler>();
        services.AddScoped<GetStudentInterventionById.Handler>();
        services.AddScoped<GetStudentInterventionPage.Handler>();
        services.AddScoped<UpdateStudentIntervention.Handler>();
        services.AddScoped<DeleteStudentIntervention.Handler>();
        services.AddScoped<IValidator<CreateStudentIntervention.Request>, CreateStudentIntervention.Validator>();
        services.AddScoped<IValidator<UpdateStudentIntervention.Request>, UpdateStudentIntervention.Validator>();
        services.AddScoped<CreateStudentPerformancePrediction.Handler>();
        services.AddScoped<GetStudentPerformancePredictionById.Handler>();
        services.AddScoped<GetStudentPerformancePredictionPage.Handler>();
        services.AddScoped<UpdateStudentPerformancePrediction.Handler>();
        services.AddScoped<DeleteStudentPerformancePrediction.Handler>();
        services.AddScoped<IValidator<CreateStudentPerformancePrediction.Request>, CreateStudentPerformancePrediction.Validator>();
        services.AddScoped<IValidator<UpdateStudentPerformancePrediction.Request>, UpdateStudentPerformancePrediction.Validator>();
        services.AddScoped<CreateTeachingRecommendation.Handler>();
        services.AddScoped<GetTeachingRecommendationById.Handler>();
        services.AddScoped<GetTeachingRecommendationPage.Handler>();
        services.AddScoped<UpdateTeachingRecommendation.Handler>();
        services.AddScoped<DeleteTeachingRecommendation.Handler>();
        services.AddScoped<IValidator<CreateTeachingRecommendation.Request>, CreateTeachingRecommendation.Validator>();
        services.AddScoped<IValidator<UpdateTeachingRecommendation.Request>, UpdateTeachingRecommendation.Validator>();
        services.AddScoped<CreateTopicPerformanceInsight.Handler>();
        services.AddScoped<GetTopicPerformanceInsightById.Handler>();
        services.AddScoped<GetTopicPerformanceInsightPage.Handler>();
        services.AddScoped<UpdateTopicPerformanceInsight.Handler>();
        services.AddScoped<DeleteTopicPerformanceInsight.Handler>();
        services.AddScoped<IValidator<CreateTopicPerformanceInsight.Request>, CreateTopicPerformanceInsight.Validator>();
        services.AddScoped<IValidator<UpdateTopicPerformanceInsight.Request>, UpdateTopicPerformanceInsight.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapAIPredictionEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateClassPerformanceInsight.MapEndpoint(endpoints);
        GetClassPerformanceInsightById.MapEndpoint(endpoints);
        GetClassPerformanceInsightPage.MapEndpoint(endpoints);
        UpdateClassPerformanceInsight.MapEndpoint(endpoints);
        DeleteClassPerformanceInsight.MapEndpoint(endpoints);
        CreatePredictionEvaluation.MapEndpoint(endpoints);
        GetPredictionEvaluationById.MapEndpoint(endpoints);
        GetPredictionEvaluationPage.MapEndpoint(endpoints);
        UpdatePredictionEvaluation.MapEndpoint(endpoints);
        DeletePredictionEvaluation.MapEndpoint(endpoints);
        CreatePredictionEvidence.MapEndpoint(endpoints);
        GetPredictionEvidenceById.MapEndpoint(endpoints);
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
        GetStudentInterventionPage.MapEndpoint(endpoints);
        UpdateStudentIntervention.MapEndpoint(endpoints);
        DeleteStudentIntervention.MapEndpoint(endpoints);
        CreateStudentPerformancePrediction.MapEndpoint(endpoints);
        GetStudentPerformancePredictionById.MapEndpoint(endpoints);
        GetStudentPerformancePredictionPage.MapEndpoint(endpoints);
        UpdateStudentPerformancePrediction.MapEndpoint(endpoints);
        DeleteStudentPerformancePrediction.MapEndpoint(endpoints);
        CreateTeachingRecommendation.MapEndpoint(endpoints);
        GetTeachingRecommendationById.MapEndpoint(endpoints);
        GetTeachingRecommendationPage.MapEndpoint(endpoints);
        UpdateTeachingRecommendation.MapEndpoint(endpoints);
        DeleteTeachingRecommendation.MapEndpoint(endpoints);
        CreateTopicPerformanceInsight.MapEndpoint(endpoints);
        GetTopicPerformanceInsightById.MapEndpoint(endpoints);
        GetTopicPerformanceInsightPage.MapEndpoint(endpoints);
        UpdateTopicPerformanceInsight.MapEndpoint(endpoints);
        DeleteTopicPerformanceInsight.MapEndpoint(endpoints);

        return endpoints;
    }
}
