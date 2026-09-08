using Dapper;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetCities
{
    public sealed record Request(int ProvinceId) : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(int Id, string Code, string Name);

    public interface IGetCitiesQuery { Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken); }
    internal sealed class GetCitiesQuery(IDbConnectionFactory connectionFactory) : IGetCitiesQuery
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Request request, CancellationToken cancellationToken)
        {
            const string sql = "SELECT city_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.city WHERE province_id = @ProvinceId ORDER BY name";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, new { request.ProvinceId }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetCitiesQuery query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(request, cancellationToken);
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/lookups/geography/cities", async (int provinceId, IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(provinceId), cancellationToken))).RequireAuthorization();
}
