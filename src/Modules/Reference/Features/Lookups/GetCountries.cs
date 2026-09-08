using Dapper;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetCountries
{
    public sealed record Request : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(int Id, string Code, string Name);

    public interface IGetCountriesQuery { Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class GetCountriesQuery(IDbConnectionFactory connectionFactory) : IGetCountriesQuery
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT country_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.country  ORDER BY name";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, null, cancellationToken: cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetCountriesQuery query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request, cancellationToken);
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/lookups/geography/countries", async (IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(), cancellationToken))).RequireAuthorization();
}
