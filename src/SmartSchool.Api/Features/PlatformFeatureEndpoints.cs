using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SmartSchool.Api.Features;

public static class PlatformFeatureEndpoints
{
    public sealed record FeatureFlag(string Key, bool Enabled, string? Description);
    public sealed record SaveFeaturesRequest(IReadOnlyCollection<FeatureFlag> Features);

    public static IEndpointRouteBuilder MapPlatformFeatureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/platform/features")
            .WithTags("Platform - Features")
            .RequireAuthorization("SuperAdminOnly");

        group.MapGet("/{tenantId:guid}", GetAsync);
        group.MapPut("/{tenantId:guid}", SaveAsync);
        return endpoints;
    }

    private static async Task<IResult> GetAsync(Guid tenantId, IDistributedCache cache, CancellationToken ct)
    {
        var json = await cache.GetStringAsync($"tenant:{tenantId}:features", ct);
        return Results.Ok(json is null
            ? DefaultFeatures()
            : JsonSerializer.Deserialize<FeatureFlag[]>(json) ?? DefaultFeatures());
    }

    private static async Task<IResult> SaveAsync(Guid tenantId, SaveFeaturesRequest request, IDistributedCache cache, CancellationToken ct)
    {
        await cache.SetStringAsync($"tenant:{tenantId}:features", JsonSerializer.Serialize(request.Features),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) }, ct);
        return Results.Ok(request.Features);
    }

    private static FeatureFlag[] DefaultFeatures() =>
    [
        new("chat", true, "Teacher/parent/student/staff discussions"),
        new("chatbot", true, "Ollama + RAG school assistant"),
        new("prediction", true, "Student, fee and academic predictions"),
        new("workflow", true, "Approvals and school workflows"),
        new("transport", true, "Vehicle, route and driver operations"),
        new("finance", true, "Fees, collection and income"),
        new("examinations", true, "Examiner and examination workspace")
    ];
}
