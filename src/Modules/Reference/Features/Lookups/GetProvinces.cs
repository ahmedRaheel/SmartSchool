using Dapper;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetProvinces
{
    public sealed record Request(int CountryId) : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(int Id, string Code, string Name);

    public interface IGetProvincesQuery { Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class GetProvincesQuery(IDbConnectionFactory connectionFactory) : IGetProvincesQuery
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT province_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.province WHERE country_id = @CountryId ORDER BY name";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, new { request.CountryId }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetProvincesQuery query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request, cancellationToken);
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/lookups/geography/provinces", async (int countryId, IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(countryId), cancellationToken))).RequireAuthorization();
}
