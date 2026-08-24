using System.Net.Http.Json;
using System.Text.Json;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.Modules.AIInquiry.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIInquiry.Features;

public static class OperationalInquiryEndpoints
{
    public sealed record StartRequest(Guid? TenantId,string VisitorName,string? Email,string? Phone,string? Language);
    public sealed record MessageRequest(Guid? TenantId,Guid ConversationId,string Message,string? Language);
    public sealed record LeadRequest(Guid? TenantId,Guid ConversationId,string Name,string? Email,string? Phone,string? Interest);
    public sealed record HandoffRequest(Guid? TenantId,Guid ConversationId,string Reason,string? PreferredContact);

    public static IEndpointRouteBuilder MapOperationalInquiryEndpoints(this IEndpointRouteBuilder e)
    {
        var g=e.MapGroup("/api/aiinquiry/operations").WithTags("AI Inquiry Operations").RequireAuthorization();
        g.MapPost("/conversations",Start);
        g.MapPost("/messages",Message);
        g.MapPost("/leads",Lead);
        g.MapPost("/handoff",Handoff);
        return e;
    }
    private static Guid? Tenant(ITenantScope s,Guid? t)=>s.IsSuperAdmin?t:s.Resolve(t);
    private static async Task<IResult> Start(StartRequest r,ITenantScope scope,IInquiryConversationCommand cmd,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var e=InquiryConversationEntity.Create(t.Value,$"INQ-{Guid.NewGuid():N}",r.VisitorName,JsonSerializer.Serialize(r));await cmd.AddAsync(e,ct);
        return Results.Created($"/api/aiinquiry/inquiry-conversation/{e.Id}",new{conversationId=e.Id,e.TenantId});
    }
    private static async Task<IResult> Message(MessageRequest r,ITenantScope scope,IInquiryMessageCommand cmd,IHttpClientFactory http,IConfiguration cfg,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var user=InquiryMessageEntity.Create(t.Value,$"MSG-{Guid.NewGuid():N}","Visitor",JsonSerializer.Serialize(new{r.ConversationId,role="user",content=r.Message,r.Language}));await cmd.AddAsync(user,ct);
        var h=http.CreateClient();h.BaseAddress=new Uri((cfg["AI:Ollama:BaseUrl"] ?? throw new InvalidOperationException("AI:Ollama:BaseUrl configuration is required.")).TrimEnd('/')+"/");
        var prompt=$"You are SmartSchool admissions inquiry assistant. Answer conservatively. Never invent eligibility, fee, policy, admission dates or school facts. If verified school knowledge is unavailable, explicitly request human handoff. Visitor message: {r.Message}";
        var resp=await h.PostAsJsonAsync("api/generate",new{model=cfg["AI:Ollama:ChatModel"]??"llama3.2",prompt,stream=false},ct);
        resp.EnsureSuccessStatusCode();
        using var doc=JsonDocument.Parse(await resp.Content.ReadAsStringAsync(ct));
        var answer=doc.RootElement.TryGetProperty("response",out var value)?value.GetString()??"":"";
        var ai=InquiryMessageEntity.Create(t.Value,$"MSG-{Guid.NewGuid():N}","Assistant",JsonSerializer.Serialize(new{r.ConversationId,role="assistant",content=answer}));await cmd.AddAsync(ai,ct);
        await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new{tenantId=t.Value,bot="admissions",conversationId=r.ConversationId},ct);
        return Results.Ok(new{messageId=ai.Id,answer});
    }
    private static async Task<IResult> Lead(LeadRequest r,ITenantScope scope,ILeadCaptureCommand cmd,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var e=LeadCaptureEntity.Create(t.Value,$"LEAD-{Guid.NewGuid():N}",r.Name,JsonSerializer.Serialize(r));await cmd.AddAsync(e,ct);
        await events.PublishAsync("smartschool.aiinquiry.lead-captured",new{tenantId=t.Value,leadId=e.Id,r.ConversationId},ct);return Results.Ok(new{leadId=e.Id});
    }
    private static async Task<IResult> Handoff(HandoffRequest r,ITenantScope scope,IHumanHandoffCommand cmd,IIntegrationEventPublisher events,CancellationToken ct)
    {
        var t=Tenant(scope,r.TenantId);if(!t.HasValue)return Results.BadRequest(new{message="Tenant required."});
        var e=HumanHandoffEntity.Create(t.Value,$"HANDOFF-{Guid.NewGuid():N}","Counselor handoff",JsonSerializer.Serialize(r));await cmd.AddAsync(e,ct);
        await events.PublishAsync("smartschool.aiinquiry.handoff-requested",new{tenantId=t.Value,handoffId=e.Id,r.ConversationId,r.Reason},ct);return Results.Accepted(value:new{handoffId=e.Id,status="Requested"});
    }
}
