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

        services.AddFeaturePersistence(typeof(Module).Assembly);
		services.AddScoped<IExamPredictionService, MlNetExamPredictionService>();
		services.AddScoped<IPredictionSuiteService, MlNetPredictionSuiteService>();
		services.AddScoped<IClassPerformanceInsightCommand, ClassPerformanceInsightCommand>();
		services.AddScoped<IClassPerformanceInsightQuery, ClassPerformanceInsightQuery>();
		services.AddScoped<IPredictionEvaluationCommand, PredictionEvaluationCommand>();
		services.AddScoped<IPredictionEvaluationQuery, PredictionEvaluationQuery>();
		services.AddScoped<IPredictionEvidenceCommand, PredictionEvidenceCommand>();
		services.AddScoped<IPredictionEvidenceQuery, PredictionEvidenceQuery>();
		services.AddScoped<IPredictionModelCommand, PredictionModelCommand>();
		services.AddScoped<IPredictionModelQuery, PredictionModelQuery>();
		services.AddScoped<IStudentInterventionCommand, StudentInterventionCommand>();
		services.AddScoped<IStudentInterventionQuery, StudentInterventionQuery>();
		services.AddScoped<IStudentPerformancePredictionCommand, StudentPerformancePredictionCommand>();
		services.AddScoped<IStudentPerformancePredictionQuery, StudentPerformancePredictionQuery>();
		services.AddScoped<ITeachingRecommendationCommand, TeachingRecommendationCommand>();
		services.AddScoped<ITeachingRecommendationQuery, TeachingRecommendationQuery>();
		services.AddScoped<ITopicPerformanceInsightCommand, TopicPerformanceInsightCommand>();
		services.AddScoped<ITopicPerformanceInsightQuery, TopicPerformanceInsightQuery>();
		services.AddScoped<IPredictionSuiteService, MlNetPredictionSuiteService>();
		services.AddScoped<IExamPredictionService, MlNetExamPredictionService>();
		services.AddScoped<IPredictionSuiteService, MlNetPredictionSuiteService>();
	
		return services;
	}

	public static IEndpointRouteBuilder MapAIPredictionEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateClassPerformanceInsight.MapEndpoint(endpoints);
		PredictExamPerformance.MapEndpoint(endpoints);
		PredictionSuiteEndpoints.MapEndpoints(endpoints);
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
