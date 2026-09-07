using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Documents.Features.GetRequiredDocuments;
public static class GetRequiredDocuments
{
 public sealed record Response(Guid Id,string ActorType,string? StaffType,string DocumentType,string DisplayName,bool IsRequired,string? ConditionCode,int MinCount,int SortOrder);
 public static void MapEndpoint(IEndpointRouteBuilder e)=>e.MapGet("/api/documents/files/requirements/{actorType}",HandleAsync).WithTags("Documents").RequireAuthorization();
 private static async Task<IResult> HandleAsync(string actorType,string? staffType,Guid? tenantId,ITenantScope scope,IDbConnectionFactory factory,CancellationToken cancellationToken){var tenant=scope.Resolve(tenantId);if(!tenant.HasValue)return Results.BadRequest(new{message="Tenant is required for SuperAdmin."});const string sql="""SELECT required_document_id AS "Id",actor_type AS "ActorType",staff_type AS "StaffType",document_type AS "DocumentType",display_name AS "DisplayName",is_required AS "IsRequired",condition_code AS "ConditionCode",min_count AS "MinCount",sort_order AS "SortOrder" FROM document.required_document WHERE is_active=true AND actor_type=@ActorType AND (tenant_id IS NULL OR tenant_id=@TenantId) AND (staff_type IS NULL OR staff_type=@StaffType) ORDER BY CASE WHEN tenant_id=@TenantId THEN 0 ELSE 1 END,sort_order,display_name""";await using var connection=await factory.OpenConnectionAsync(cancellationToken);return Results.Ok(await connection.QueryAsync<Response>(new CommandDefinition(sql,new{TenantId=tenant.Value,ActorType=actorType.ToUpperInvariant(),StaffType=staffType?.ToUpperInvariant()},cancellationToken:cancellationToken)));}
}
