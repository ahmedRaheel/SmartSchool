using SmartSchool.Modules.AITutor.Persistence;
using FluentValidation;
using SmartSchool.Modules.AITutor.Features.GeneratedQuiz;
using SmartSchool.Modules.AITutor.Features.LearningRecommendation;
using SmartSchool.Modules.AITutor.Features.QuizAttempt;
using SmartSchool.Modules.AITutor.Features.StudentTopicMastery;
using SmartSchool.Modules.AITutor.Features.TutorConversation;
using SmartSchool.Modules.AITutor.Features.TutorMessage;
using SmartSchool.Modules.AITutor.Features.TutorSession;

namespace SmartSchool.Modules.AITutor;

public static class Module
{
    public static IServiceCollection AddAITutorModule(
        this IServiceCollection services)
    {
        services.AddScoped<IGeneratedQuizQuery, GeneratedQuizQuery>();
        services.AddScoped<IGeneratedQuizCommand, GeneratedQuizCommand>();
        services.AddScoped<ILearningRecommendationQuery, LearningRecommendationQuery>();
        services.AddScoped<ILearningRecommendationCommand, LearningRecommendationCommand>();
        services.AddScoped<IQuizAttemptQuery, QuizAttemptQuery>();
        services.AddScoped<IQuizAttemptCommand, QuizAttemptCommand>();
        services.AddScoped<IStudentTopicMasteryQuery, StudentTopicMasteryQuery>();
        services.AddScoped<IStudentTopicMasteryCommand, StudentTopicMasteryCommand>();
        services.AddScoped<ITutorConversationQuery, TutorConversationQuery>();
        services.AddScoped<ITutorConversationCommand, TutorConversationCommand>();
        services.AddScoped<ITutorMessageQuery, TutorMessageQuery>();
        services.AddScoped<ITutorMessageCommand, TutorMessageCommand>();
        services.AddScoped<ITutorSessionQuery, TutorSessionQuery>();
        services.AddScoped<ITutorSessionCommand, TutorSessionCommand>();

        services.AddScoped<CreateGeneratedQuiz.Handler>();
        services.AddScoped<GetGeneratedQuizById.Handler>();
        services.AddScoped<GetGeneratedQuizPage.Handler>();
        services.AddScoped<UpdateGeneratedQuiz.Handler>();
        services.AddScoped<DeleteGeneratedQuiz.Handler>();
        services.AddScoped<IValidator<CreateGeneratedQuiz.Request>, CreateGeneratedQuiz.Validator>();
        services.AddScoped<IValidator<UpdateGeneratedQuiz.Request>, UpdateGeneratedQuiz.Validator>();
        services.AddScoped<CreateLearningRecommendation.Handler>();
        services.AddScoped<GetLearningRecommendationById.Handler>();
        services.AddScoped<GetLearningRecommendationPage.Handler>();
        services.AddScoped<UpdateLearningRecommendation.Handler>();
        services.AddScoped<DeleteLearningRecommendation.Handler>();
        services.AddScoped<IValidator<CreateLearningRecommendation.Request>, CreateLearningRecommendation.Validator>();
        services.AddScoped<IValidator<UpdateLearningRecommendation.Request>, UpdateLearningRecommendation.Validator>();
        services.AddScoped<CreateQuizAttempt.Handler>();
        services.AddScoped<GetQuizAttemptById.Handler>();
        services.AddScoped<GetQuizAttemptPage.Handler>();
        services.AddScoped<UpdateQuizAttempt.Handler>();
        services.AddScoped<DeleteQuizAttempt.Handler>();
        services.AddScoped<IValidator<CreateQuizAttempt.Request>, CreateQuizAttempt.Validator>();
        services.AddScoped<IValidator<UpdateQuizAttempt.Request>, UpdateQuizAttempt.Validator>();
        services.AddScoped<CreateStudentTopicMastery.Handler>();
        services.AddScoped<GetStudentTopicMasteryById.Handler>();
        services.AddScoped<GetStudentTopicMasteryPage.Handler>();
        services.AddScoped<UpdateStudentTopicMastery.Handler>();
        services.AddScoped<DeleteStudentTopicMastery.Handler>();
        services.AddScoped<IValidator<CreateStudentTopicMastery.Request>, CreateStudentTopicMastery.Validator>();
        services.AddScoped<IValidator<UpdateStudentTopicMastery.Request>, UpdateStudentTopicMastery.Validator>();
        services.AddScoped<CreateTutorConversation.Handler>();
        services.AddScoped<GetTutorConversationById.Handler>();
        services.AddScoped<GetTutorConversationPage.Handler>();
        services.AddScoped<UpdateTutorConversation.Handler>();
        services.AddScoped<DeleteTutorConversation.Handler>();
        services.AddScoped<IValidator<CreateTutorConversation.Request>, CreateTutorConversation.Validator>();
        services.AddScoped<IValidator<UpdateTutorConversation.Request>, UpdateTutorConversation.Validator>();
        services.AddScoped<CreateTutorMessage.Handler>();
        services.AddScoped<GetTutorMessageById.Handler>();
        services.AddScoped<GetTutorMessagePage.Handler>();
        services.AddScoped<UpdateTutorMessage.Handler>();
        services.AddScoped<DeleteTutorMessage.Handler>();
        services.AddScoped<IValidator<CreateTutorMessage.Request>, CreateTutorMessage.Validator>();
        services.AddScoped<IValidator<UpdateTutorMessage.Request>, UpdateTutorMessage.Validator>();
        services.AddScoped<CreateTutorSession.Handler>();
        services.AddScoped<GetTutorSessionById.Handler>();
        services.AddScoped<GetTutorSessionPage.Handler>();
        services.AddScoped<UpdateTutorSession.Handler>();
        services.AddScoped<DeleteTutorSession.Handler>();
        services.AddScoped<IValidator<CreateTutorSession.Request>, CreateTutorSession.Validator>();
        services.AddScoped<IValidator<UpdateTutorSession.Request>, UpdateTutorSession.Validator>();

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

        return endpoints;
    }
}
