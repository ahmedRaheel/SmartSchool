using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Persistence;
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
        services.AddScoped<IClassPerformanceInsightQuery, ClassPerformanceInsightQuery>();
        services.AddScoped<IClassPerformanceInsightCommand, ClassPerformanceInsightCommand>();
        services.AddScoped<IPredictionEvaluationQuery, PredictionEvaluationQuery>();
        services.AddScoped<IPredictionEvaluationCommand, PredictionEvaluationCommand>();
        services.AddScoped<IPredictionEvidenceQuery, PredictionEvidenceQuery>();
        services.AddScoped<IPredictionEvidenceCommand, PredictionEvidenceCommand>();
        services.AddScoped<IPredictionModelQuery, PredictionModelQuery>();
        services.AddScoped<IPredictionModelCommand, PredictionModelCommand>();
        services.AddScoped<IStudentInterventionQuery, StudentInterventionQuery>();
        services.AddScoped<IStudentInterventionCommand, StudentInterventionCommand>();
        services.AddScoped<IStudentPerformancePredictionQuery, StudentPerformancePredictionQuery>();
        services.AddScoped<IStudentPerformancePredictionCommand, StudentPerformancePredictionCommand>();
        services.AddScoped<ITeachingRecommendationQuery, TeachingRecommendationQuery>();
        services.AddScoped<ITeachingRecommendationCommand, TeachingRecommendationCommand>();
        services.AddScoped<ITopicPerformanceInsightQuery, TopicPerformanceInsightQuery>();
        services.AddScoped<ITopicPerformanceInsightCommand, TopicPerformanceInsightCommand>();
        services.AddScoped<IValidator<CreateClassPerformanceInsight.Request>, CreateClassPerformanceInsight.Validator>();
        services.AddScoped<IValidator<UpdateClassPerformanceInsight.Request>, UpdateClassPerformanceInsight.Validator>();
        services.AddScoped<IValidator<CreatePredictionEvaluation.Request>, CreatePredictionEvaluation.Validator>();
        services.AddScoped<IValidator<UpdatePredictionEvaluation.Request>, UpdatePredictionEvaluation.Validator>();
        services.AddScoped<IValidator<CreatePredictionEvidence.Request>, CreatePredictionEvidence.Validator>();
        services.AddScoped<IValidator<UpdatePredictionEvidence.Request>, UpdatePredictionEvidence.Validator>();
        services.AddScoped<IValidator<CreatePredictionModel.Request>, CreatePredictionModel.Validator>();
        services.AddScoped<IValidator<UpdatePredictionModel.Request>, UpdatePredictionModel.Validator>();
        services.AddScoped<IValidator<CreateStudentIntervention.Request>, CreateStudentIntervention.Validator>();
        services.AddScoped<IValidator<UpdateStudentIntervention.Request>, UpdateStudentIntervention.Validator>();
        services.AddScoped<IValidator<CreateStudentPerformancePrediction.Request>, CreateStudentPerformancePrediction.Validator>();
        services.AddScoped<IValidator<UpdateStudentPerformancePrediction.Request>, UpdateStudentPerformancePrediction.Validator>();
        services.AddScoped<IValidator<CreateTeachingRecommendation.Request>, CreateTeachingRecommendation.Validator>();
        services.AddScoped<IValidator<UpdateTeachingRecommendation.Request>, UpdateTeachingRecommendation.Validator>();
        services.AddScoped<IValidator<CreateTopicPerformanceInsight.Request>, CreateTopicPerformanceInsight.Validator>();
        services.AddScoped<IValidator<UpdateTopicPerformanceInsight.Request>, UpdateTopicPerformanceInsight.Validator>();


        services.AddScoped<IRequestHandler<CreateClassPerformanceInsight.Request, Result<ClassPerformanceInsightResponse>>, CreateClassPerformanceInsight.Handler>();
        services.AddScoped<IRequestHandler<GetClassPerformanceInsightById.Query, Result<ClassPerformanceInsightResponse>>, GetClassPerformanceInsightById.Handler>();
        services.AddScoped<IRequestHandler<GetClassPerformanceInsightPage.Query, Result<PagedResult<ClassPerformanceInsightResponse>>>, GetClassPerformanceInsightPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateClassPerformanceInsight.Request, Result<ClassPerformanceInsightResponse>>, UpdateClassPerformanceInsight.Handler>();
        services.AddScoped<IRequestHandler<DeleteClassPerformanceInsight.Command, Result<DeleteClassPerformanceInsight.Response>>, DeleteClassPerformanceInsight.Handler>();
        services.AddScoped<IRequestHandler<CreatePredictionEvaluation.Request, Result<PredictionEvaluationResponse>>, CreatePredictionEvaluation.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionEvaluationById.Query, Result<PredictionEvaluationResponse>>, GetPredictionEvaluationById.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionEvaluationPage.Query, Result<PagedResult<PredictionEvaluationResponse>>>, GetPredictionEvaluationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePredictionEvaluation.Request, Result<PredictionEvaluationResponse>>, UpdatePredictionEvaluation.Handler>();
        services.AddScoped<IRequestHandler<DeletePredictionEvaluation.Command, Result<DeletePredictionEvaluation.Response>>, DeletePredictionEvaluation.Handler>();
        services.AddScoped<IRequestHandler<CreatePredictionEvidence.Request, Result<PredictionEvidenceResponse>>, CreatePredictionEvidence.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionEvidenceById.Query, Result<PredictionEvidenceResponse>>, GetPredictionEvidenceById.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionEvidencePage.Query, Result<PagedResult<PredictionEvidenceResponse>>>, GetPredictionEvidencePage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePredictionEvidence.Request, Result<PredictionEvidenceResponse>>, UpdatePredictionEvidence.Handler>();
        services.AddScoped<IRequestHandler<DeletePredictionEvidence.Command, Result<DeletePredictionEvidence.Response>>, DeletePredictionEvidence.Handler>();
        services.AddScoped<IRequestHandler<CreatePredictionModel.Request, Result<PredictionModelResponse>>, CreatePredictionModel.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionModelById.Query, Result<PredictionModelResponse>>, GetPredictionModelById.Handler>();
        services.AddScoped<IRequestHandler<GetPredictionModelPage.Query, Result<PagedResult<PredictionModelResponse>>>, GetPredictionModelPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePredictionModel.Request, Result<PredictionModelResponse>>, UpdatePredictionModel.Handler>();
        services.AddScoped<IRequestHandler<DeletePredictionModel.Command, Result<DeletePredictionModel.Response>>, DeletePredictionModel.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentIntervention.Request, Result<StudentInterventionResponse>>, CreateStudentIntervention.Handler>();
        services.AddScoped<IRequestHandler<GetStudentInterventionById.Query, Result<StudentInterventionResponse>>, GetStudentInterventionById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentInterventionPage.Query, Result<PagedResult<StudentInterventionResponse>>>, GetStudentInterventionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentIntervention.Request, Result<StudentInterventionResponse>>, UpdateStudentIntervention.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentIntervention.Command, Result<DeleteStudentIntervention.Response>>, DeleteStudentIntervention.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentPerformancePrediction.Request, Result<StudentPerformancePredictionResponse>>, CreateStudentPerformancePrediction.Handler>();
        services.AddScoped<IRequestHandler<GetStudentPerformancePredictionById.Query, Result<StudentPerformancePredictionResponse>>, GetStudentPerformancePredictionById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentPerformancePredictionPage.Query, Result<PagedResult<StudentPerformancePredictionResponse>>>, GetStudentPerformancePredictionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentPerformancePrediction.Request, Result<StudentPerformancePredictionResponse>>, UpdateStudentPerformancePrediction.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentPerformancePrediction.Command, Result<DeleteStudentPerformancePrediction.Response>>, DeleteStudentPerformancePrediction.Handler>();
        services.AddScoped<IRequestHandler<CreateTeachingRecommendation.Request, Result<TeachingRecommendationResponse>>, CreateTeachingRecommendation.Handler>();
        services.AddScoped<IRequestHandler<GetTeachingRecommendationById.Query, Result<TeachingRecommendationResponse>>, GetTeachingRecommendationById.Handler>();
        services.AddScoped<IRequestHandler<GetTeachingRecommendationPage.Query, Result<PagedResult<TeachingRecommendationResponse>>>, GetTeachingRecommendationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTeachingRecommendation.Request, Result<TeachingRecommendationResponse>>, UpdateTeachingRecommendation.Handler>();
        services.AddScoped<IRequestHandler<DeleteTeachingRecommendation.Command, Result<DeleteTeachingRecommendation.Response>>, DeleteTeachingRecommendation.Handler>();
        services.AddScoped<IRequestHandler<CreateTopicPerformanceInsight.Request, Result<TopicPerformanceInsightResponse>>, CreateTopicPerformanceInsight.Handler>();
        services.AddScoped<IRequestHandler<GetTopicPerformanceInsightById.Query, Result<TopicPerformanceInsightResponse>>, GetTopicPerformanceInsightById.Handler>();
        services.AddScoped<IRequestHandler<GetTopicPerformanceInsightPage.Query, Result<PagedResult<TopicPerformanceInsightResponse>>>, GetTopicPerformanceInsightPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTopicPerformanceInsight.Request, Result<TopicPerformanceInsightResponse>>, UpdateTopicPerformanceInsight.Handler>();
        services.AddScoped<IRequestHandler<DeleteTopicPerformanceInsight.Command, Result<DeleteTopicPerformanceInsight.Response>>, DeleteTopicPerformanceInsight.Handler>();

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
