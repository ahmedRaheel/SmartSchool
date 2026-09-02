using Dapper;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetGeography
{
    public sealed record Response(int Id, string Code, string Name);

    public sealed record GetCountries : IRequest<IReadOnlyList<Response>>;
    public sealed record GetProvinces(int CountryId) : IRequest<IReadOnlyList<Response>>;
    public sealed record GetCities(int ProvinceId) : IRequest<IReadOnlyList<Response>>;

    public interface IGetGeography
    {
        Task<IReadOnlyList<Response>> GetCountriesAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<Response>> GetProvincesAsync(int countryId, CancellationToken cancellationToken);
        Task<IReadOnlyList<Response>> GetCitiesAsync(int provinceId, CancellationToken cancellationToken);
    }

    internal sealed class Persistence(IDbConnectionFactory connectionFactory) : IGetGeography
    {
        public Task<IReadOnlyList<Response>> GetCountriesAsync(CancellationToken cancellationToken) =>
            QueryAsync("SELECT country_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.country ORDER BY name", null, cancellationToken);

        public Task<IReadOnlyList<Response>> GetProvincesAsync(int countryId, CancellationToken cancellationToken) =>
            QueryAsync("SELECT province_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.province WHERE country_id=@CountryId ORDER BY name", new { CountryId = countryId }, cancellationToken);

        public Task<IReadOnlyList<Response>> GetCitiesAsync(int provinceId, CancellationToken cancellationToken) =>
            QueryAsync("SELECT city_id AS \"Id\", code AS \"Code\", name AS \"Name\" FROM reference.city WHERE province_id=@ProvinceId ORDER BY name", new { ProvinceId = provinceId }, cancellationToken);

        private async Task<IReadOnlyList<Response>> QueryAsync(string sql, object? parameters, CancellationToken cancellationToken)
        {
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken))).AsList();
        }
    }

    public sealed class CountriesHandler(IGetGeography query) : IRequestHandler<GetCountries, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(GetCountries request, CancellationToken cancellationToken) => query.GetCountriesAsync(cancellationToken);
    }

    public sealed class ProvincesHandler(IGetGeography query) : IRequestHandler<GetProvinces, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(GetProvinces request, CancellationToken cancellationToken) => query.GetProvincesAsync(request.CountryId, cancellationToken);
    }

    public sealed class CitiesHandler(IGetGeography query) : IRequestHandler<GetCities, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(GetCities request, CancellationToken cancellationToken) => query.GetCitiesAsync(request.ProvinceId, cancellationToken);
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/lookups/geography/countries", async (IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<GetCountries, IReadOnlyList<Response>>(new GetCountries(), ct))).RequireAuthorization();
        endpoints.MapGet("/api/lookups/geography/provinces", async (int countryId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<GetProvinces, IReadOnlyList<Response>>(new GetProvinces(countryId), ct))).RequireAuthorization();
        endpoints.MapGet("/api/lookups/geography/cities", async (int provinceId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<GetCities, IReadOnlyList<Response>>(new GetCities(provinceId), ct))).RequireAuthorization();
    }
}
