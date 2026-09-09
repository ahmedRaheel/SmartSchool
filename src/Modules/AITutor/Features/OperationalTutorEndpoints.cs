using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Application.Persistence;
using System.Text.Json;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.AI;

namespace SmartSchool.Modules.AITutor.Features;

public static class OperationalTutorEndpoints
{
    public sealed record StartSessionRequest(
        Guid? TenantId,
        Guid StudentId,
        string Subject,
        string? Topic);

    public sealed record AskRequest(
        Guid? TenantId,
        Guid SessionId,
        Guid StudentId,
        string Subject,
        string Topic,
        string Message);

    public sealed record QuizRequest(
        Guid? TenantId,
        Guid StudentId,
        string Subject,
        string Topic,
        int QuestionCount = 5,
        string Difficulty = "adaptive");

    public sealed record RecommendationRequest(
        Guid? TenantId,
        Guid StudentId,
        string Subject,
        string Topic,
        double MasteryScore);

    public static IEndpointRouteBuilder MapOperationalTutorEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/aitutor/operations")
            .WithTags("AI Tutor Operations")
            .RequireAuthorization();

        group.MapPost("/sessions", Start);
        group.MapPost("/ask", Ask);
        group.MapPost("/quizzes/generate", Quiz);
        group.MapPost("/recommendations/generate", Recommend);

        return endpoints;
    }

    private static Guid? ResolveTenantId(
        ITenantScope tenantScope,
        Guid? requestedTenantId)
    {
        return tenantScope.IsSuperAdmin
            ? requestedTenantId
            : tenantScope.Resolve(requestedTenantId);
    }

    private static async Task<IResult> Start(
        StartSessionRequest request,
        ITenantScope tenantScope,
        OperationalTutorEndpointsTutorSessionCommand sessionCommand,
        OperationalTutorEndpointsTutorConversationCommand conversationCommand,
        IBusinessNumberGenerator numberGenerator,
        CancellationToken cancellationToken)
    {
        var tenantId = ResolveTenantId(tenantScope, request.TenantId);
        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant required." });
        }

        var sessionCode = await numberGenerator.NextAsync(
            "TutorSession",
            "SESSION",
            tenantId.Value,
            6,
            cancellationToken);

        var session = TutorSessionEntity.Create(
            tenantId.Value,
            sessionCode,
            $"{request.Subject}: {request.Topic}",
            JsonSerializer.Serialize(request));

        await sessionCommand.AddAsync(session, cancellationToken);

        var conversationCode = await numberGenerator.NextAsync(
            "TutorConversation",
            "CONV",
            tenantId.Value,
            6,
            cancellationToken);

        var conversation = TutorConversationEntity.Create(
            tenantId.Value,
            conversationCode,
            $"{request.Subject} tutoring",
            JsonSerializer.Serialize(new
            {
                sessionId = session.TutorSessionId,
                request.StudentId,
                request.Subject,
                request.Topic
            }));

        await conversationCommand.AddAsync(conversation, cancellationToken);

        return Results.Created(
            $"/api/aitutor/tutor-session/{session.TutorSessionId}",
            new
            {
                sessionId = session.TutorSessionId,
                conversationId = conversation.TutorConversationId
            });
    }

    private static async Task<IResult> Ask(
        AskRequest request,
        ITenantScope tenantScope,
        OperationalTutorEndpointsTutorMessageCommand messageCommand,
        IOllamaClient ollamaClient,
        IIntegrationEventPublisher eventPublisher,
        IBusinessNumberGenerator numberGenerator,
        CancellationToken cancellationToken)
    {
        var tenantId = ResolveTenantId(tenantScope, request.TenantId);
        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant required." });
        }

        var userMessageCode = await numberGenerator.NextAsync(
            "TutorMessage",
            "TMSG",
            tenantId.Value,
            6,
            cancellationToken);

        var userMessage = TutorMessageEntity.Create(
            tenantId.Value,
            userMessageCode,
            SmartSchoolRoles.Student,
            JsonSerializer.Serialize(new
            {
                request.SessionId,
                request.StudentId,
                role = "user",
                content = request.Message,
                request.Subject,
                request.Topic
            }));

        await messageCommand.AddAsync(userMessage, cancellationToken);

        var prompt = $"""
            You are SmartSchool AI Tutor. Student subject: {request.Subject}. Topic: {request.Topic}.

            Teach using hints, explanation and formative questions. Do not fabricate school-specific facts.
            Do not reveal another student's data. For assessed work, coach rather than blindly completing it.
            Student: {request.Message}
            """;

        var (answer, model) = await ollamaClient.GenerateAsync(
            prompt,
            cancellationToken);

        var assistantMessageCode = await numberGenerator.NextAsync(
            "TutorMessage",
            "TMSG",
            tenantId.Value,
            6,
            cancellationToken);

        var assistantMessage = TutorMessageEntity.Create(
            tenantId.Value,
            assistantMessageCode,
            "AI Tutor",
            JsonSerializer.Serialize(new
            {
                request.SessionId,
                request.StudentId,
                role = "assistant",
                content = answer
            }));

        await messageCommand.AddAsync(assistantMessage, cancellationToken);

        await eventPublisher.PublishAsync(
            KafkaTopics.ChatbotQuestionAsked,
            new
            {
                tenantId = tenantId.Value,
                bot = "student-tutor",
                request.StudentId,
                request.SessionId
            },
            cancellationToken);

        return Results.Ok(new
        {
            messageId = assistantMessage.TutorMessageId,
            answer,
            model
        });
    }

    private static async Task<IResult> Quiz(
        QuizRequest request,
        ITenantScope tenantScope,
        OperationalTutorEndpointsGeneratedQuizCommand quizCommand,
        IOllamaClient ollamaClient,
        IIntegrationEventPublisher eventPublisher,
        IBusinessNumberGenerator numberGenerator,
        CancellationToken cancellationToken)
    {
        var tenantId = ResolveTenantId(tenantScope, request.TenantId);
        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant required." });
        }

        var questionCount = Math.Clamp(request.QuestionCount, 1, 20);
        var prompt = $"""
            Generate exactly {questionCount} {request.Difficulty} quiz questions for {request.Subject}, topic {request.Topic}.

            Return ONLY valid JSON array. Each object: question, options (4 strings), correctAnswer, explanation.
            Avoid personal data.
            """;

        var generatedContent = (await ollamaClient.GenerateAsync(
            prompt,
            cancellationToken)).Answer;

        var quizCode = await numberGenerator.NextAsync(
            "GeneratedQuiz",
            "QUIZ",
            tenantId.Value,
            6,
            cancellationToken);

        var quiz = GeneratedQuizEntity.Create(
            tenantId.Value,
            quizCode,
            $"{request.Subject} - {request.Topic}",
            JsonSerializer.Serialize(new
            {
                request.StudentId,
                request.Subject,
                request.Topic,
                request.Difficulty,
                questionsJson = generatedContent
            }));

        await quizCommand.AddAsync(quiz, cancellationToken);

        await eventPublisher.PublishAsync(
            "smartschool.aitutor.quiz-generated",
            new
            {
                tenantId = tenantId.Value,
                quizId = quiz.GeneratedQuizId,
                request.StudentId
            },
            cancellationToken);

        return Results.Created(
            $"/api/aitutor/generated-quiz/{quiz.GeneratedQuizId}",
            new
            {
                quizId = quiz.GeneratedQuizId,
                questions = TryJson(generatedContent)
            });
    }

    private static async Task<IResult> Recommend(
        RecommendationRequest request,
        ITenantScope tenantScope,
        OperationalTutorEndpointsLearningRecommendationCommand recommendationCommand,
        IOllamaClient ollamaClient,
        IBusinessNumberGenerator numberGenerator,
        CancellationToken cancellationToken)
    {
        var tenantId = ResolveTenantId(tenantScope, request.TenantId);
        if (!tenantId.HasValue)
        {
            return Results.BadRequest(new { message = "Tenant required." });
        }

        var prompt =
            $"Create a concise learning plan for {request.Subject}/{request.Topic}. " +
            $"Current mastery is {request.MasteryScore:P0}. Include next concept, practice type, " +
            "revision frequency and success criterion.";

        var (answer, _) = await ollamaClient.GenerateAsync(
            prompt,
            cancellationToken);

        var recommendationCode = await numberGenerator.NextAsync(
            "LearningRecommendation",
            "REC",
            tenantId.Value,
            6,
            cancellationToken);

        var recommendation = LearningRecommendationEntity.Create(
            tenantId.Value,
            recommendationCode,
            $"{request.Subject} recommendation",
            JsonSerializer.Serialize(new
            {
                request.StudentId,
                request.Subject,
                request.Topic,
                request.MasteryScore,
                recommendation = answer
            }));

        await recommendationCommand.AddAsync(
            recommendation,
            cancellationToken);

        return Results.Ok(new
        {
            recommendationId = recommendation.LearningRecommendationId,
            recommendation = answer
        });
    }

    private static object TryJson(string raw)
    {
        try
        {
            return JsonSerializer.Deserialize<object>(raw) ?? raw;
        }
        catch (JsonException)
        {
            return raw;
        }
    }
}

/// <summary>
/// Feature-owned data access for OperationalTutorEndpoints. Do not share across slices.
/// </summary>
public sealed class OperationalTutorEndpointsTutorMessageCommand(IAITutorDbContext dbContext)
{
    public async Task AddAsync(
        TutorMessageEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.TutorMessages
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for OperationalTutorEndpoints. Do not share across slices.
/// </summary>
public sealed class OperationalTutorEndpointsTutorSessionCommand(IAITutorDbContext dbContext)
{
    public async Task AddAsync(
        TutorSessionEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.TutorSessions
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for OperationalTutorEndpoints. Do not share across slices.
/// </summary>
public sealed class OperationalTutorEndpointsLearningRecommendationCommand(IAITutorDbContext dbContext)
{
    public async Task AddAsync(
        LearningRecommendationEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.LearningRecommendations
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for OperationalTutorEndpoints. Do not share across slices.
/// </summary>
public sealed class OperationalTutorEndpointsTutorConversationCommand(IAITutorDbContext dbContext)
{
    public async Task AddAsync(
        TutorConversationEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.TutorConversations
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

/// <summary>
/// Feature-owned data access for OperationalTutorEndpoints. Do not share across slices.
/// </summary>
public sealed class OperationalTutorEndpointsGeneratedQuizCommand(IAITutorDbContext dbContext)
{
    public async Task AddAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.GeneratedQuizs
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
