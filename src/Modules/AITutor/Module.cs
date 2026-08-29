using SmartSchool.Modules.AITutor.Features;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Features.GeneratedQuiz;
using SmartSchool.Modules.AITutor.Features.LearningRecommendation;
using SmartSchool.Modules.AITutor.Features.QuizAttempt;
using SmartSchool.Modules.AITutor.Features.StudentTopicMastery;
using SmartSchool.Modules.AITutor.Features.TutorConversation;
using SmartSchool.Modules.AITutor.Features.TutorMessage;
using SmartSchool.Modules.AITutor.Features.TutorSession;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor;

public static class Module
{
	public static IServiceCollection AddAITutorModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeatureDataAccess(typeof(Module).Assembly);
		return services;
	}

	public static IEndpointRouteBuilder MapAITutorEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateGeneratedQuiz.MapEndpoint(endpoints);
		GetGeneratedQuizById.MapEndpoint(endpoints);
		GetGeneratedQuizPage.MapEndpoint(endpoints);
		UpdateGeneratedQuiz.MapEndpoint(endpoints);
		DeleteGeneratedQuiz.MapEndpoint(endpoints);
		CreateLearningRecommendation.MapEndpoint(endpoints);
		GetLearningRecommendationById.MapEndpoint(endpoints);
		GetLearningRecommendationPage.MapEndpoint(endpoints);
		UpdateLearningRecommendation.MapEndpoint(endpoints);
		DeleteLearningRecommendation.MapEndpoint(endpoints);
		CreateQuizAttempt.MapEndpoint(endpoints);
		GetQuizAttemptById.MapEndpoint(endpoints);
		GetQuizAttemptPage.MapEndpoint(endpoints);
		UpdateQuizAttempt.MapEndpoint(endpoints);
		DeleteQuizAttempt.MapEndpoint(endpoints);
		CreateStudentTopicMastery.MapEndpoint(endpoints);
		GetStudentTopicMasteryById.MapEndpoint(endpoints);
		GetStudentTopicMasteryPage.MapEndpoint(endpoints);
		UpdateStudentTopicMastery.MapEndpoint(endpoints);
		DeleteStudentTopicMastery.MapEndpoint(endpoints);
		CreateTutorConversation.MapEndpoint(endpoints);
		GetTutorConversationById.MapEndpoint(endpoints);
		GetTutorConversationPage.MapEndpoint(endpoints);
		UpdateTutorConversation.MapEndpoint(endpoints);
		DeleteTutorConversation.MapEndpoint(endpoints);
		CreateTutorMessage.MapEndpoint(endpoints);
		GetTutorMessageById.MapEndpoint(endpoints);
		GetTutorMessagePage.MapEndpoint(endpoints);
		UpdateTutorMessage.MapEndpoint(endpoints);
		DeleteTutorMessage.MapEndpoint(endpoints);
		CreateTutorSession.MapEndpoint(endpoints);
		GetTutorSessionById.MapEndpoint(endpoints);
		GetTutorSessionPage.MapEndpoint(endpoints);
		UpdateTutorSession.MapEndpoint(endpoints);
		DeleteTutorSession.MapEndpoint(endpoints);

		OperationalTutorEndpoints.MapOperationalTutorEndpoints(endpoints);

		return endpoints;
	}
}
