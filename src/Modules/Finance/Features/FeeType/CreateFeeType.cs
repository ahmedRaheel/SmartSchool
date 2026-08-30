using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeType;

public static class CreateFeeType
{
    public sealed record Response(Guid TenantId, Guid Id, string Code, string Name, string Frequency, bool IsActive, string? Description);
    public sealed record Request(Guid TenantId, string Name, string Frequency = "Monthly", string? Description = null) : IRequest<Result<Response>>;
    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator() { RuleFor(x=>x.TenantId).NotEmpty(); RuleFor(x=>x.Name).NotEmpty().MaximumLength(120); RuleFor(x=>x.Frequency).NotEmpty().Must(x=>new[]{"Monthly","Term","Annual","OneTime"}.Contains(x)); }
    }
    public interface ICreateFeeType { Task AddAsync(FeeTypeEntity entity, CancellationToken cancellationToken); }
    internal sealed class CreateFeeTypePersistence(IApplicationDbContext dbContext) : ICreateFeeType
    {
        public async Task AddAsync(FeeTypeEntity entity, CancellationToken cancellationToken) { await dbContext.Set<FeeTypeEntity>().AddAsync(entity,cancellationToken); await dbContext.SaveChangesAsync(cancellationToken); }
    }
    public sealed class Handler(ICreateFeeType persistence) : IRequestHandler<Request,Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request,CancellationToken cancellationToken)
        {
            var code = $"FEE-{Guid.NewGuid():N}"[..12].ToUpperInvariant();
            var entity=FeeTypeEntity.Create(request.TenantId,code,request.Name,request.Frequency,request.Description);
            await persistence.AddAsync(entity,cancellationToken);
            return Result<Response>.Success(new(entity.TenantId,entity.FeeTypeId,entity.Code,entity.Name,entity.Frequency,entity.IsActive,entity.Description));
        }
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(ApiRoutes.EntityCollection(ModuleConstants.RouteSegment,"fee-type"),async(Request request,IMediator mediator,CancellationToken ct)=>(await mediator.SendAsync<Request,Result<Response>>(request,ct)).ToHttpResult()).WithName("CreateFeeType").WithTags(ModuleConstants.Name).RequireAuthorization(); return endpoints;
    }
}
