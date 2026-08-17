using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateGeneratedQuiz.Request>, CreateGeneratedQuiz.Validator>();
        services.AddScoped<IValidator<UpdateGeneratedQuiz.Request>, UpdateGeneratedQuiz.Validator>();
        services.AddScoped<IValidator<CreateLearningRecommendation.Request>, CreateLearningRecommendation.Validator>();
        services.AddScoped<IValidator<UpdateLearningRecommendation.Request>, UpdateLearningRecommendation.Validator>();
        services.AddScoped<IValidator<CreateQuizAttempt.Request>, CreateQuizAttempt.Validator>();
        services.AddScoped<IValidator<UpdateQuizAttempt.Request>, UpdateQuizAttempt.Validator>();
        services.AddScoped<IValidator<CreateStudentTopicMastery.Request>, CreateStudentTopicMastery.Validator>();
        services.AddScoped<IValidator<UpdateStudentTopicMastery.Request>, UpdateStudentTopicMastery.Validator>();
        services.AddScoped<IValidator<CreateTutorConversation.Request>, CreateTutorConversation.Validator>();
        services.AddScoped<IValidator<UpdateTutorConversation.Request>, UpdateTutorConversation.Validator>();
        services.AddScoped<IValidator<CreateTutorMessage.Request>, CreateTutorMessage.Validator>();
        services.AddScoped<IValidator<UpdateTutorMessage.Request>, UpdateTutorMessage.Validator>();
        services.AddScoped<IValidator<CreateTutorSession.Request>, CreateTutorSession.Validator>();
        services.AddScoped<IValidator<UpdateTutorSession.Request>, UpdateTutorSession.Validator>();


        services.AddScoped<IRequestHandler<CreateGeneratedQuiz.Request, Result<GeneratedQuizResponse>>, CreateGeneratedQuiz.Handler>();
        services.AddScoped<IRequestHandler<GetGeneratedQuizById.Query, Result<GeneratedQuizResponse>>, GetGeneratedQuizById.Handler>();
        services.AddScoped<IRequestHandler<GetGeneratedQuizPage.Query, Result<PagedResult<GeneratedQuizResponse>>>, GetGeneratedQuizPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateGeneratedQuiz.Request, Result<GeneratedQuizResponse>>, UpdateGeneratedQuiz.Handler>();
        services.AddScoped<IRequestHandler<DeleteGeneratedQuiz.Command, Result<DeleteGeneratedQuiz.Response>>, DeleteGeneratedQuiz.Handler>();
        services.AddScoped<IRequestHandler<CreateLearningRecommendation.Request, Result<LearningRecommendationResponse>>, CreateLearningRecommendation.Handler>();
        services.AddScoped<IRequestHandler<GetLearningRecommendationById.Query, Result<LearningRecommendationResponse>>, GetLearningRecommendationById.Handler>();
        services.AddScoped<IRequestHandler<GetLearningRecommendationPage.Query, Result<PagedResult<LearningRecommendationResponse>>>, GetLearningRecommendationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLearningRecommendation.Request, Result<LearningRecommendationResponse>>, UpdateLearningRecommendation.Handler>();
        services.AddScoped<IRequestHandler<DeleteLearningRecommendation.Command, Result<DeleteLearningRecommendation.Response>>, DeleteLearningRecommendation.Handler>();
        services.AddScoped<IRequestHandler<CreateQuizAttempt.Request, Result<QuizAttemptResponse>>, CreateQuizAttempt.Handler>();
        services.AddScoped<IRequestHandler<GetQuizAttemptById.Query, Result<QuizAttemptResponse>>, GetQuizAttemptById.Handler>();
        services.AddScoped<IRequestHandler<GetQuizAttemptPage.Query, Result<PagedResult<QuizAttemptResponse>>>, GetQuizAttemptPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateQuizAttempt.Request, Result<QuizAttemptResponse>>, UpdateQuizAttempt.Handler>();
        services.AddScoped<IRequestHandler<DeleteQuizAttempt.Command, Result<DeleteQuizAttempt.Response>>, DeleteQuizAttempt.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentTopicMastery.Request, Result<StudentTopicMasteryResponse>>, CreateStudentTopicMastery.Handler>();
        services.AddScoped<IRequestHandler<GetStudentTopicMasteryById.Query, Result<StudentTopicMasteryResponse>>, GetStudentTopicMasteryById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentTopicMasteryPage.Query, Result<PagedResult<StudentTopicMasteryResponse>>>, GetStudentTopicMasteryPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentTopicMastery.Request, Result<StudentTopicMasteryResponse>>, UpdateStudentTopicMastery.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentTopicMastery.Command, Result<DeleteStudentTopicMastery.Response>>, DeleteStudentTopicMastery.Handler>();
        services.AddScoped<IRequestHandler<CreateTutorConversation.Request, Result<TutorConversationResponse>>, CreateTutorConversation.Handler>();
        services.AddScoped<IRequestHandler<GetTutorConversationById.Query, Result<TutorConversationResponse>>, GetTutorConversationById.Handler>();
        services.AddScoped<IRequestHandler<GetTutorConversationPage.Query, Result<PagedResult<TutorConversationResponse>>>, GetTutorConversationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTutorConversation.Request, Result<TutorConversationResponse>>, UpdateTutorConversation.Handler>();
        services.AddScoped<IRequestHandler<DeleteTutorConversation.Command, Result<DeleteTutorConversation.Response>>, DeleteTutorConversation.Handler>();
        services.AddScoped<IRequestHandler<CreateTutorMessage.Request, Result<TutorMessageResponse>>, CreateTutorMessage.Handler>();
        services.AddScoped<IRequestHandler<GetTutorMessageById.Query, Result<TutorMessageResponse>>, GetTutorMessageById.Handler>();
        services.AddScoped<IRequestHandler<GetTutorMessagePage.Query, Result<PagedResult<TutorMessageResponse>>>, GetTutorMessagePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTutorMessage.Request, Result<TutorMessageResponse>>, UpdateTutorMessage.Handler>();
        services.AddScoped<IRequestHandler<DeleteTutorMessage.Command, Result<DeleteTutorMessage.Response>>, DeleteTutorMessage.Handler>();
        services.AddScoped<IRequestHandler<CreateTutorSession.Request, Result<TutorSessionResponse>>, CreateTutorSession.Handler>();
        services.AddScoped<IRequestHandler<GetTutorSessionById.Query, Result<TutorSessionResponse>>, GetTutorSessionById.Handler>();
        services.AddScoped<IRequestHandler<GetTutorSessionPage.Query, Result<PagedResult<TutorSessionResponse>>>, GetTutorSessionPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateTutorSession.Request, Result<TutorSessionResponse>>, UpdateTutorSession.Handler>();
        services.AddScoped<IRequestHandler<DeleteTutorSession.Command, Result<DeleteTutorSession.Response>>, DeleteTutorSession.Handler>();

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
