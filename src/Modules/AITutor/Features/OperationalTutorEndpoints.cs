using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using System.Text.Json;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Features.TutorSession;
using SmartSchool.Modules.AITutor.Features.TutorConversation;
using SmartSchool.Modules.AITutor.Features.TutorMessage;
using SmartSchool.Modules.AITutor.Features.GeneratedQuiz;
using SmartSchool.Modules.AITutor.Features.LearningRecommendation;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.AI;

namespace SmartSchool.Modules.AITutor.Features;

public static class OperationalTutorEndpoints
{
    public sealed record StartSessionRequest(Guid? TenantId,Guid StudentId,string Subject,string? Topic);
    public sealed record AskRequest(Guid? TenantId,Guid SessionId,Guid StudentId,string Subject,string Topic,string Message);
    public sealed record QuizRequest(Guid? TenantId,Guid StudentId,string Subject,string Topic,int QuestionCount=5,string Difficulty="adaptive");
    public sealed record RecommendationRequest(Guid? TenantId,Guid StudentId,string Subject,string Topic,double MasteryScore);

    public static IEndpointRouteBuilder MapOperationalTutorEndpoints(this IEndpointRouteBuilder e)
    {
        var g=e.MapGroup("/api/aitutor/operations").WithTags("AI Tutor Operations").RequireAuthorization();
        g.MapPost("/sessions",Start);
        g.MapPost("/ask",Ask);
        g.MapPost("/quizzes/generate",Quiz);
        g.MapPost("/recommendations/generate",Recommend);
        return e;
    }
    private static Guid? Tenant(ITenantScope s,Guid? t)=>s.IsSuperAdmin?t:s.Resolve(t);
    private static async Task<IResult> Start(StartSessionRequest r,ITenantScope scope,OperationalTutorEndpointsTutorSessionCommand sessions,OperationalTutorEndpointsTutorConversationCommand conversations,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var s=TutorSessionEntity.Create(t.Value,$"SESSION-{Guid.NewGuid():N}",$"{r.Subject}: {r.Topic}",JsonSerializer.Serialize(r));await sessions.AddAsync(s,ct);
        var c=TutorConversationEntity.Create(t.Value,$"CONV-{Guid.NewGuid():N}",$"{r.Subject} tutoring",JsonSerializer.Serialize(new{sessionId=s.TutorSessionId,r.StudentId,r.Subject,r.Topic}));await conversations.AddAsync(c,ct);
        return Results.Created($"/api/aitutor/tutor-session/{s.TutorSessionId}",new{sessionId=s.TutorSessionId,conversationId=c.TutorConversationId});
    }
    private static async Task<IResult> Ask(AskRequest r,ITenantScope scope,OperationalTutorEndpointsTutorMessageCommand messages,IOllamaClient ollama,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var u=TutorMessageEntity.Create(t.Value,$"TMSG-{Guid.NewGuid():N}",SmartSchoolRoles.Student,JsonSerializer.Serialize(new{r.SessionId,r.StudentId,role="user",content=r.Message,r.Subject,r.Topic}));await messages.AddAsync(u,ct);
        var prompt=$"""
            You are SmartSchool AI Tutor. Student subject: {r.Subject}. Topic: {r.Topic}.

Teach using hints, explanation and formative questions. Do not fabricate school-specific facts. Do not reveal another student's data.
For assessed work, coach rather than blindly completing it.
Student: {r.Message}
""";
        var generated=await ollama.GenerateAsync(prompt,ct);
        var answer=generated.Answer;
        var a=TutorMessageEntity.Create(t.Value,$"TMSG-{Guid.NewGuid():N}","AI Tutor",JsonSerializer.Serialize(new{r.SessionId,r.StudentId,role="assistant",content=answer}));await messages.AddAsync(a,ct);
        await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new{tenantId=t.Value,bot="student-tutor",r.StudentId,r.SessionId},ct);
        return Results.Ok(new{messageId=a.TutorMessageId,answer,model=generated.Model});
    }
    private static async Task<IResult> Quiz(QuizRequest r,ITenantScope scope,OperationalTutorEndpointsGeneratedQuizCommand quizzes,IOllamaClient ollama,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var count=Math.Clamp(r.QuestionCount,1,20);
        var prompt=$"""
            Generate exactly {count} {r.Difficulty} quiz questions for {r.Subject}, topic {r.Topic}.

Return ONLY valid JSON array. Each object: question, options (4 strings), correctAnswer, explanation. Avoid personal data.
""";
        var raw=(await ollama.GenerateAsync(prompt,ct)).Answer;
        var e=GeneratedQuizEntity.Create(t.Value,$"QUIZ-{Guid.NewGuid():N}",$"{r.Subject} - {r.Topic}",JsonSerializer.Serialize(new{r.StudentId,r.Subject,r.Topic,r.Difficulty,questionsJson=raw}));
        await quizzes.AddAsync(e,ct);await events.PublishAsync("smartschool.aitutor.quiz-generated",new{tenantId=t.Value,quizId=e.GeneratedQuizId,r.StudentId},ct);
        return Results.Created($"/api/aitutor/generated-quiz/{e.GeneratedQuizId}",new{quizId=e.GeneratedQuizId,questions=TryJson(raw)});
    }
    private static async Task<IResult> Recommend(RecommendationRequest r,ITenantScope scope,OperationalTutorEndpointsLearningRecommendationCommand recommendations,IOllamaClient ollama,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var prompt=$"Create a concise learning plan for {r.Subject}/{r.Topic}. Current mastery is {r.MasteryScore:P0}. Include next concept, practice type, revision frequency and success criterion.";
        var generated=await ollama.GenerateAsync(prompt,ct);
        var answer=generated.Answer;
        var e=LearningRecommendationEntity.Create(t.Value,$"REC-{Guid.NewGuid():N}",$"{r.Subject} recommendation",JsonSerializer.Serialize(new{r.StudentId,r.Subject,r.Topic,r.MasteryScore,recommendation=answer}));
        await recommendations.AddAsync(e,ct);return Results.Ok(new{recommendationId=e.LearningRecommendationId,recommendation=answer});
    }
    private static object TryJson(string raw){try{return JsonSerializer.Deserialize<object>(raw)??raw;}catch{return raw;}}
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
