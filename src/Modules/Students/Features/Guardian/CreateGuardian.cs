using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.Students.Features.Guardian;

public static class CreateGuardian
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid? UserId,
        string FullName,
        string? CnicNumber,
        string? Email,
        string? Phone,
        LoginAccountResponse LoginAccount);

    public sealed record LoginAccountResponse(
        Guid UserId,
        string Email,
        string TemporaryPassword,
        bool MustChangePassword);

    public sealed record Request(
        Guid TenantId,
        Guid? UserId,
        string FullName,
        string? CnicNumber,
        string? Email,
        string? Phone) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.CnicNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        }
    }

    public interface ICreateGuardianCommand
    {
        Task AddAsync(
                GuardianEntity entity,
                CancellationToken cancellationToken);

        Task<bool> ExistsByCnicNumberAsync(Guid tenantId, string cnicNumber, Guid? excludingId, CancellationToken cancellationToken);

    }

    internal sealed class CreateGuardianCommand(
        IStudentsDbContext dbContext) : ICreateGuardianCommand
    {
        public async Task AddAsync(
                GuardianEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.Guardians
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public Task<bool> ExistsByCnicNumberAsync(
            Guid tenantId, string cnicNumber, Guid? excludingId, CancellationToken cancellationToken)
        {
            return dbContext.Guardians.AnyAsync(
                x => x.TenantId == tenantId && x.CnicNumber == cnicNumber
                    && (!excludingId.HasValue || x.GuardianId != excludingId.Value), cancellationToken);
        }
}

    public sealed class Handler(
        ICreateGuardianCommand command,
        IIdentityAccountService identityAccountService)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var exists = !string.IsNullOrWhiteSpace(request.CnicNumber)
                && await command.ExistsByCnicNumberAsync(
                    request.TenantId, request.CnicNumber, null, cancellationToken);
            if (exists)
            {
                return Result<Response>.Failure(
                    Error.Conflict("Guardian with the supplied CnicNumber already exists."));
            }

            var entity = GuardianEntity.Create(
                request.TenantId,
                request.UserId,
                request.FullName,
                request.CnicNumber,
                request.Email,
                request.Phone);

            var nameParts = request.FullName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts[0];
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

            ProvisionedAccount account;
            try
            {
                account = await identityAccountService.CreateAccountAsync(
                    request.TenantId,
                    entity.GuardianId,
                    SmartSchoolRoles.Parent,
                    request.Email!,
                    firstName,
                    lastName,
                    null,
                    null,
                    [SmartSchoolRoles.Parent],
                    cancellationToken);
            }
            catch (Exception exception)
            {
                return Result<Response>.Failure(
                    Error.InternalServerError($"Unable to create the parent login account: {exception.Message}"));
            }

            entity.LinkIdentityAccount(account.UserId);
            try
            {
                await command.AddAsync(entity, cancellationToken);
            }
            catch
            {
                await identityAccountService.DeleteAccountAsync(account.UserId, cancellationToken);
                throw;
            }

            return Result<Response>.Success(MapResponse(entity, account));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "guardian"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateGuardian").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantStudent);
        return endpoints;
    }

    private static Response MapResponse(GuardianEntity entity, ProvisionedAccount account)
    {
        return new Response(
            entity.TenantId,
            entity.GuardianId,
            entity.UserId,
            entity.FullName,
            entity.CnicNumber,
            entity.Email,
            entity.Phone,
            new LoginAccountResponse(account.UserId, account.Email, account.TemporaryPassword, account.MustChangePassword));
    }
}
