using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.DocumentSetup;

public static class CreateRequiredDocument
{
    public sealed record Request(string UserRole, bool IsMandatory, Guid RequiredDocumentTypeId, Guid? TenantId = null) : IRequest<Result<Response>>;
    public sealed record Response(Guid RequiredDocumentId, string UserRole, bool IsMandatory, Guid RequiredDocumentTypeId);

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.UserRole).NotEmpty().MaximumLength(50);
            RuleFor(request => request.RequiredDocumentTypeId).NotEmpty();
        }
    }

    public interface ICreateRequiredDocumentQuery { Task<bool> IsValidAsync(Guid tenantId, Guid typeId, CancellationToken cancellationToken); }
    internal sealed class CreateRequiredDocumentQuery(IDbConnectionFactory factory) : ICreateRequiredDocumentQuery
    {
        public async Task<bool> IsValidAsync(Guid tenantId, Guid typeId, CancellationToken cancellationToken)
        {
            await using var connection = await factory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("SELECT EXISTS (SELECT 1 FROM document.required_document_type WHERE tenant_id = @TenantId AND required_document_type_id = @TypeId AND is_active)", new { TenantId = tenantId, TypeId = typeId }, cancellationToken: cancellationToken));
        }
    }
    public interface ICreateRequiredDocumentCommand
    {
        Task AddAsync(RequiredDocumentEntity requiredDocument, CancellationToken cancellationToken);
    }

    internal sealed class CreateRequiredDocumentCommand(IDocumentsDbContext dbContext) : ICreateRequiredDocumentCommand
    {
        public async Task AddAsync(RequiredDocumentEntity requiredDocument, CancellationToken cancellationToken)
        {
            await dbContext.RequiredDocuments.AddAsync(requiredDocument, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(ICreateRequiredDocumentCommand command, ICreateRequiredDocumentQuery query, ICurrentUser currentUser, ITenantScope scope)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            if (scope.Resolve(request.TenantId) is not Guid tenantId)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            if (!await query.IsValidAsync(tenantId, request.RequiredDocumentTypeId, cancellationToken)) return Result<Response>.Failure(Error.Validation("Choose an active required document type in this tenant."));
            var entity = RequiredDocumentEntity.Create(
                tenantId,
                currentUser.BranchId,
                request.UserRole,
                request.IsMandatory,
                request.RequiredDocumentTypeId);
            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(entity.RequiredDocumentId, entity.UserRole, entity.IsMandatory, entity.RequiredDocumentTypeId));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/documents/required-documents", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithName("CreateRequiredDocument").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
        return endpoints;
    }
}
