using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetBranchGenderTypes
{
    public sealed record Request : IRequest<Result<IReadOnlyCollection<Response>>>;
    public sealed record Response(Guid Id, string Code, string Name);
    public interface IGetBranchGenderTypesQuery { Task<IReadOnlyCollection<Response>> ExecuteAsync(CancellationToken cancellationToken); }
    internal sealed class GetBranchGenderTypesQuery(IDbConnectionFactory connectionFactory) : IGetBranchGenderTypesQuery { public async Task<IReadOnlyCollection<Response>> ExecuteAsync(CancellationToken cancellationToken) { const string sql = "SELECT branch_gender_type_id AS Id, code AS Code, name AS Name FROM reference.branch_gender_type WHERE is_active = TRUE ORDER BY sort_order, name;"; await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<Response>(new CommandDefinition(sql, cancellationToken: cancellationToken))).AsList(); } }
    public sealed class Handler(IGetBranchGenderTypesQuery query) : IRequestHandler<Request, Result<IReadOnlyCollection<Response>>> { public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Request request, CancellationToken cancellationToken) => Result<IReadOnlyCollection<Response>>.Success(await query.ExecuteAsync(cancellationToken)); }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/organization/lookups/branch-gender-types", async (IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Request, Result<IReadOnlyCollection<Response>>>(new Request(), cancellationToken)).ToHttpResult()).WithTags(ModuleConstants.Name).RequireAuthorization();
}
