using Dapper;
using SmartSchool.Application.Persistence;
namespace SmartSchool.Modules.Reference.Features.Lookups;
public static class GetGeography
{
 public static void MapEndpoint(IEndpointRouteBuilder endpoints)
 {
  endpoints.MapGet("/api/lookups/geography/countries", async (IDbConnectionFactory f,CancellationToken ct)=>{await using var db=await f.OpenConnectionAsync(ct);return Results.Ok(await db.QueryAsync(new CommandDefinition("SELECT country_id AS Id, code AS Code, name AS Name FROM reference.country ORDER BY name",cancellationToken:ct)));}).RequireAuthorization();
  endpoints.MapGet("/api/lookups/geography/provinces", async (int countryId,IDbConnectionFactory f,CancellationToken ct)=>{await using var db=await f.OpenConnectionAsync(ct);return Results.Ok(await db.QueryAsync(new CommandDefinition("SELECT province_id AS Id, code AS Code, name AS Name FROM reference.province WHERE country_id=@countryId ORDER BY name",new{countryId},cancellationToken:ct)));}).RequireAuthorization();
  endpoints.MapGet("/api/lookups/geography/cities", async (int provinceId,IDbConnectionFactory f,CancellationToken ct)=>{await using var db=await f.OpenConnectionAsync(ct);return Results.Ok(await db.QueryAsync(new CommandDefinition("SELECT city_id AS Id, code AS Code, name AS Name FROM reference.city WHERE province_id=@provinceId ORDER BY name",new{provinceId},cancellationToken:ct)));}).RequireAuthorization();
 }
}
