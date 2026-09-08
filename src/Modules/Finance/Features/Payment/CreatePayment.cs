using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Payment;

public static class CreatePayment
{
    /// <summary>
    /// Represents the response returned by this PaymentEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreatePaymentCommand
    {
        Task AddAsync(
                PaymentEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreatePaymentCommand(IFinanceDbContext dbContext) : ICreatePaymentCommand
    {
        public async Task AddAsync(
                PaymentEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.Payments
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreatePaymentCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("Payment", "PAY", request.TenantId, 3, cancellationToken);

            var entity = PaymentEntity.Create(
                request.TenantId,
                code,
                request.Name);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "payment"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreatePayment")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(PaymentEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.StudentPaymentId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
