using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Features.Certificates;

/// <summary>
/// Versioned certificate/letter templates and immutable issued-document
/// snapshots with verification codes.
/// </summary>
public static class CertificateOperations
{
    public sealed record TemplateItem(
        Guid TemplateId,
        Guid? CampusId,
        string DocumentTypeCode,
        string Code,
        string Name,
        string BodyHtml,
        string? HeaderHtml,
        string? FooterHtml,
        string LanguageCode,
        int Version,
        bool RequiresApproval);

    public sealed record IssuedItem(
        Guid GeneratedDocumentId,
        Guid TemplateId,
        string TemplateName,
        string DocumentTypeCode,
        string DocumentNumber,
        Guid? StudentId,
        Guid? EmployeeId,
        string OwnerName,
        string Status,
        string? VerificationCode,
        DateTimeOffset? IssuedAt,
        Guid? ApprovedBy);

    public sealed record DashboardQuery(Guid TenantId) : IRequest<Result<DashboardResponse>>;
    public sealed record DashboardResponse(
        IReadOnlyList<TemplateItem> Templates,
        IReadOnlyList<IssuedItem> IssuedDocuments);

    public interface ICertificateDashboardQuery
    {
        Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class CertificateDashboardQuery(IDbConnectionFactory connectionFactory)
        : ICertificateDashboardQuery
    {
        public async Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    t.document_template_id AS "TemplateId",
                    t.campus_id AS "CampusId",
                    t.document_type_code AS "DocumentTypeCode",
                    t.code AS "Code",
                    t.name AS "Name",
                    t.body_html AS "BodyHtml",
                    t.header_html AS "HeaderHtml",
                    t.footer_html AS "FooterHtml",
                    t.language_code AS "LanguageCode",
                    t.version AS "Version",
                    t.requires_approval AS "RequiresApproval"
                FROM document.document_template t
                WHERE t.tenant_id = @TenantId
                  AND t.is_active
                  AND (@CampusId IS NULL OR t.campus_id IS NULL OR t.campus_id = @CampusId)
                ORDER BY t.document_type_code, t.name, t.version DESC;

                SELECT
                    g.generated_document_id AS "GeneratedDocumentId",
                    g.document_template_id AS "TemplateId",
                    t.name AS "TemplateName",
                    t.document_type_code AS "DocumentTypeCode",
                    g.document_number AS "DocumentNumber",
                    g.student_id AS "StudentId",
                    g.employee_id AS "EmployeeId",
                    CASE
                        WHEN s.student_id IS NOT NULL THEN trim(s.first_name || ' ' || coalesce(s.last_name, ''))
                        WHEN e.employee_id IS NOT NULL THEN trim(e.first_name || ' ' || coalesce(e.last_name, ''))
                        ELSE 'Unknown owner'
                    END AS "OwnerName",
                    g.status AS "Status",
                    g.verification_code AS "VerificationCode",
                    g.issued_at AS "IssuedAt",
                    g.approved_by AS "ApprovedBy"
                FROM document.generated_document g
                JOIN document.document_template t
                  ON t.document_template_id = g.document_template_id
                 AND t.tenant_id = g.tenant_id
                LEFT JOIN student.student s
                  ON s.student_id = g.student_id
                 AND s.tenant_id = g.tenant_id
                LEFT JOIN hr.employee e
                  ON e.employee_id = g.employee_id
                 AND e.tenant_id = g.tenant_id
                WHERE g.tenant_id = @TenantId
                  AND g.is_active
                  AND (@CampusId IS NULL OR t.campus_id IS NULL OR t.campus_id = @CampusId)
                ORDER BY g.created_at DESC
                LIMIT 500;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId },
                    cancellationToken: cancellationToken));

            return new DashboardResponse(
                (await grid.ReadAsync<TemplateItem>()).AsList(),
                (await grid.ReadAsync<IssuedItem>()).AsList());
        }
    }

    public sealed class DashboardHandler(
        ICertificateDashboardQuery query,
        ICurrentUser currentUser) : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
    {
        public async Task<Result<DashboardResponse>> HandleAsync(
            DashboardQuery request,
            CancellationToken cancellationToken) =>
            Result<DashboardResponse>.Success(
                await query.GetAsync(request.TenantId, currentUser.BranchId, cancellationToken));
    }

    public sealed record CreateTemplateRequest(
        Guid TenantId,
        Guid? CampusId,
        Guid? AcademicSystemId,
        string DocumentTypeCode,
        string Code,
        string Name,
        string? SubjectTemplate,
        string? HeaderHtml,
        string BodyHtml,
        string? FooterHtml,
        string LanguageCode = "en",
        bool RequiresApproval = false) : IRequest<Result<CreateTemplateResponse>>;

    public sealed record CreateTemplateResponse(Guid TemplateId, int Version);

    public sealed class CreateTemplateValidator : AbstractValidator<CreateTemplateRequest>
    {
        public CreateTemplateValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.DocumentTypeCode).NotEmpty().MaximumLength(50);
            RuleFor(request => request.Code).NotEmpty().MaximumLength(80);
            RuleFor(request => request.Name).NotEmpty().MaximumLength(180);
            RuleFor(request => request.BodyHtml).NotEmpty();
            RuleFor(request => request.LanguageCode).NotEmpty().MaximumLength(10);
        }
    }

    public interface ICreateCertificateTemplateQuery
    {
        Task<bool> ScopeExistsAsync(
            CreateTemplateRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);

        Task<int> NextVersionAsync(
            CreateTemplateRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateCertificateTemplateQuery(IDbConnectionFactory connectionFactory)
        : ICreateCertificateTemplateQuery
    {
        public async Task<bool> ScopeExistsAsync(
            CreateTemplateRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            if (request.CampusId.HasValue && scopedCampusId.HasValue && request.CampusId != scopedCampusId)
            {
                return false;
            }

            const string sql = """
                SELECT
                    (@CampusId IS NULL OR EXISTS (
                        SELECT 1 FROM org.campus
                        WHERE tenant_id = @TenantId
                          AND campus_id = @CampusId
                          AND is_active
                    ))
                    AND
                    (@AcademicSystemId IS NULL OR EXISTS (
                        SELECT 1 FROM academic.academic_system
                        WHERE tenant_id = @TenantId
                          AND academic_system_id = @AcademicSystemId
                          AND is_active
                    ));
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { request.TenantId, request.CampusId, request.AcademicSystemId },
                    cancellationToken: cancellationToken));
        }

        public async Task<int> NextVersionAsync(
            CreateTemplateRequest request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT coalesce(max(version), 0) + 1
                FROM document.document_template
                WHERE tenant_id = @TenantId
                  AND code = @Code;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, new { request.TenantId, request.Code }, cancellationToken: cancellationToken));
        }
    }

    public interface ICreateCertificateTemplateCommand
    {
        Task<CreateTemplateResponse> CreateAsync(
            CreateTemplateRequest request,
            int version,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateCertificateTemplateCommand(IDocumentsDbContext dbContext)
        : ICreateCertificateTemplateCommand
    {
        public async Task<CreateTemplateResponse> CreateAsync(
            CreateTemplateRequest request,
            int version,
            CancellationToken cancellationToken)
        {
            var templateId = Guid.NewGuid();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO document.document_template
                (
                    document_template_id,
                    tenant_id,
                    campus_id,
                    academic_system_id,
                    document_type_code,
                    code,
                    name,
                    subject_template,
                    header_html,
                    body_html,
                    footer_html,
                    language_code,
                    version,
                    requires_approval,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {templateId},
                    {request.TenantId},
                    {request.CampusId},
                    {request.AcademicSystemId},
                    {request.DocumentTypeCode.Trim().ToUpperInvariant()},
                    {request.Code.Trim().ToUpperInvariant()},
                    {request.Name.Trim()},
                    {request.SubjectTemplate},
                    {request.HeaderHtml},
                    {request.BodyHtml},
                    {request.FooterHtml},
                    {request.LanguageCode.Trim().ToLowerInvariant()},
                    {version},
                    {request.RequiresApproval},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);

            return new CreateTemplateResponse(templateId, version);
        }
    }

    public sealed class CreateTemplateHandler(
        ICreateCertificateTemplateQuery query,
        ICreateCertificateTemplateCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreateTemplateRequest, Result<CreateTemplateResponse>>
    {
        public async Task<Result<CreateTemplateResponse>> HandleAsync(
            CreateTemplateRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.ScopeExistsAsync(request, currentUser.BranchId, cancellationToken))
            {
                return Result<CreateTemplateResponse>.Failure(Error.Validation("Template scope is outside the current tenant/campus."));
            }

            var version = await query.NextVersionAsync(request, cancellationToken);
            return Result<CreateTemplateResponse>.Success(
                await command.CreateAsync(request, version, cancellationToken));
        }
    }

    public sealed record IssueRequest(
        Guid TenantId,
        Guid TemplateId,
        Guid? StudentId,
        Guid? EmployeeId,
        IReadOnlyDictionary<string, string?>? Fields) : IRequest<Result<IssueResponse>>;

    public sealed record IssueResponse(
        Guid GeneratedDocumentId,
        string DocumentNumber,
        string VerificationCode,
        string Status,
        string RenderedContent);

    public sealed class IssueValidator : AbstractValidator<IssueRequest>
    {
        public IssueValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.TemplateId).NotEmpty();
            RuleFor(request => request)
                .Must(request => request.StudentId.HasValue ^ request.EmployeeId.HasValue)
                .WithMessage("Exactly one document owner is required: student or employee.");
        }
    }

    public sealed record IssueContext(
        Guid TemplateId,
        int Version,
        bool RequiresApproval,
        string BodyHtml,
        string? HeaderHtml,
        string? FooterHtml,
        string OwnerName,
        string? OwnerNumber,
        Guid? CampusId,
        string? CampusName);

    public interface IIssueCertificateQuery
    {
        Task<IssueContext?> GetContextAsync(
            IssueRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);
    }

    internal sealed class IssueCertificateQuery(IDbConnectionFactory connectionFactory)
        : IIssueCertificateQuery
    {
        public async Task<IssueContext?> GetContextAsync(
            IssueRequest request,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    t.document_template_id AS "TemplateId",
                    t.version AS "Version",
                    t.requires_approval AS "RequiresApproval",
                    t.body_html AS "BodyHtml",
                    t.header_html AS "HeaderHtml",
                    t.footer_html AS "FooterHtml",
                    CASE
                        WHEN s.student_id IS NOT NULL THEN trim(s.first_name || ' ' || coalesce(s.last_name, ''))
                        WHEN e.employee_id IS NOT NULL THEN trim(e.first_name || ' ' || coalesce(e.last_name, ''))
                        ELSE ''
                    END AS "OwnerName",
                    coalesce(s.student_number, e.employee_number) AS "OwnerNumber",
                    coalesce(s.branch_id, e.branch_id, t.campus_id) AS "CampusId",
                    c.name AS "CampusName"
                FROM document.document_template t
                LEFT JOIN student.student s
                  ON s.student_id = @StudentId
                 AND s.tenant_id = t.tenant_id
                 AND s.is_active
                LEFT JOIN hr.employee e
                  ON e.employee_id = @EmployeeId
                 AND e.tenant_id = t.tenant_id
                 AND e.is_active
                LEFT JOIN org.campus c
                  ON c.campus_id = coalesce(s.branch_id, e.branch_id, t.campus_id)
                 AND c.tenant_id = t.tenant_id
                WHERE t.tenant_id = @TenantId
                  AND t.document_template_id = @TemplateId
                  AND t.is_active
                  AND (@StudentId IS NULL OR s.student_id IS NOT NULL)
                  AND (@EmployeeId IS NULL OR e.employee_id IS NOT NULL)
                  AND (@ScopedCampusId IS NULL OR coalesce(s.branch_id, e.branch_id, t.campus_id) = @ScopedCampusId)
                  AND (t.campus_id IS NULL OR t.campus_id = coalesce(s.branch_id, e.branch_id, t.campus_id));
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<IssueContext>(
                new CommandDefinition(
                    sql,
                    new
                    {
                        request.TenantId,
                        request.TemplateId,
                        request.StudentId,
                        request.EmployeeId,
                        ScopedCampusId = scopedCampusId
                    },
                    cancellationToken: cancellationToken));
        }
    }

    public interface IIssueCertificateCommand
    {
        Task<IssueResponse> IssueAsync(
            IssueRequest request,
            IssueContext context,
            Guid issuedBy,
            CancellationToken cancellationToken);
    }

    internal sealed class IssueCertificateCommand(IDocumentsDbContext dbContext)
        : IIssueCertificateCommand
    {
        public async Task<IssueResponse> IssueAsync(
            IssueRequest request,
            IssueContext context,
            Guid issuedBy,
            CancellationToken cancellationToken)
        {
            var generatedId = Guid.NewGuid();
            var documentNumber = $"DOC-{DateTime.UtcNow:yyyyMMdd}-{generatedId:N}"[..26].ToUpperInvariant();
            var verificationCode = Convert.ToHexString(Guid.NewGuid().ToByteArray());
            var status = context.RequiresApproval ? "PENDING_APPROVAL" : "ISSUED";
            var issuedAt = context.RequiresApproval ? (DateTimeOffset?)null : DateTimeOffset.UtcNow;
            var rendered = Render(context, request.Fields, documentNumber, verificationCode);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO document.generated_document
                (
                    generated_document_id,
                    tenant_id,
                    document_template_id,
                    template_version,
                    student_id,
                    employee_id,
                    document_number,
                    rendered_content_snapshot,
                    verification_code,
                    issued_by,
                    issued_at,
                    status,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {generatedId},
                    {request.TenantId},
                    {request.TemplateId},
                    {context.Version},
                    {request.StudentId},
                    {request.EmployeeId},
                    {documentNumber},
                    {rendered},
                    {verificationCode},
                    {issuedBy},
                    {issuedAt},
                    {status},
                    {documentNumber},
                    {context.OwnerName + " certificate"},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);

            return new IssueResponse(generatedId, documentNumber, verificationCode, status, rendered);
        }

        private static string Render(
            IssueContext context,
            IReadOnlyDictionary<string, string?>? fields,
            string documentNumber,
            string verificationCode)
        {
            var rendered = string.Concat(context.HeaderHtml, context.BodyHtml, context.FooterHtml);
            var replacements = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["OwnerName"] = context.OwnerName,
                ["OwnerNumber"] = context.OwnerNumber,
                ["CampusName"] = context.CampusName,
                ["DocumentNumber"] = documentNumber,
                ["VerificationCode"] = verificationCode,
                ["IssueDate"] = DateTime.UtcNow.ToString("yyyy-MM-dd")
            };

            if (fields is not null)
            {
                foreach (var field in fields)
                {
                    replacements[field.Key] = field.Value;
                }
            }

            foreach (var replacement in replacements)
            {
                rendered = rendered.Replace(
                    "{{" + replacement.Key + "}}",
                    replacement.Value ?? string.Empty,
                    StringComparison.OrdinalIgnoreCase);
            }

            return rendered;
        }
    }

    public sealed class IssueHandler(
        IIssueCertificateQuery query,
        IIssueCertificateCommand command,
        ICurrentUser currentUser) : IRequestHandler<IssueRequest, Result<IssueResponse>>
    {
        public async Task<Result<IssueResponse>> HandleAsync(
            IssueRequest request,
            CancellationToken cancellationToken)
        {
            var context = await query.GetContextAsync(
                request,
                currentUser.BranchId,
                cancellationToken);
            if (context is null)
            {
                return Result<IssueResponse>.Failure(Error.Validation("Template or document owner is outside the current scope."));
            }

            return Result<IssueResponse>.Success(
                await command.IssueAsync(request, context, currentUser.UserId, cancellationToken));
        }
    }

    public sealed record ApproveRequest(Guid TenantId, Guid GeneratedDocumentId)
        : IRequest<Result<ApproveResponse>>;

    public sealed record ApproveResponse(Guid GeneratedDocumentId, DateTimeOffset IssuedAt);

    public interface IApproveCertificateCommand
    {
        Task<Result<ApproveResponse>> ApproveAsync(
            ApproveRequest request,
            Guid approvedBy,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class ApproveCertificateCommand(IDocumentsDbContext dbContext)
        : IApproveCertificateCommand
    {
        public async Task<Result<ApproveResponse>> ApproveAsync(
            ApproveRequest request,
            Guid approvedBy,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            var issuedAt = DateTimeOffset.UtcNow;
            var updated = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE document.generated_document generated
                SET approved_by = {approvedBy},
                    issued_at = {issuedAt},
                    status = {"ISSUED"},
                    updated_at = now(),
                    row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex')
                FROM document.document_template template
                WHERE generated.document_template_id = template.document_template_id
                  AND generated.tenant_id = template.tenant_id
                  AND generated.tenant_id = {request.TenantId}
                  AND generated.generated_document_id = {request.GeneratedDocumentId}
                  AND generated.is_active
                  AND upper(generated.status) = {"PENDING_APPROVAL"}
                  AND ({campusId} IS NULL OR template.campus_id IS NULL OR template.campus_id = {campusId});
                """, cancellationToken);

            return updated == 1
                ? Result<ApproveResponse>.Success(new ApproveResponse(request.GeneratedDocumentId, issuedAt))
                : Result<ApproveResponse>.Failure(Error.Conflict("Only a pending certificate in the current scope can be approved."));
        }
    }

    public sealed class ApproveHandler(
        IApproveCertificateCommand command,
        ICurrentUser currentUser) : IRequestHandler<ApproveRequest, Result<ApproveResponse>>
    {
        public Task<Result<ApproveResponse>> HandleAsync(
            ApproveRequest request,
            CancellationToken cancellationToken) =>
            command.ApproveAsync(request, currentUser.UserId, currentUser.BranchId, cancellationToken);
    }

    public sealed record VerifyQuery(string VerificationCode) : IRequest<Result<VerifyResponse>>;
    public sealed record VerifyResponse(
        bool IsValid,
        string? DocumentNumber,
        string? DocumentTypeCode,
        string? OwnerName,
        string? Status,
        DateTimeOffset? IssuedAt);

    public interface IVerifyCertificateQuery
    {
        Task<VerifyResponse> VerifyAsync(string verificationCode, CancellationToken cancellationToken);
    }

    internal sealed class VerifyCertificateQuery(IDbConnectionFactory connectionFactory)
        : IVerifyCertificateQuery
    {
        public async Task<VerifyResponse> VerifyAsync(string verificationCode, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    TRUE AS "IsValid",
                    g.document_number AS "DocumentNumber",
                    t.document_type_code AS "DocumentTypeCode",
                    CASE
                        WHEN s.student_id IS NOT NULL THEN trim(s.first_name || ' ' || coalesce(s.last_name, ''))
                        WHEN e.employee_id IS NOT NULL THEN trim(e.first_name || ' ' || coalesce(e.last_name, ''))
                        ELSE NULL
                    END AS "OwnerName",
                    g.status AS "Status",
                    g.issued_at AS "IssuedAt"
                FROM document.generated_document g
                JOIN document.document_template t
                  ON t.document_template_id = g.document_template_id
                 AND t.tenant_id = g.tenant_id
                LEFT JOIN student.student s
                  ON s.student_id = g.student_id
                 AND s.tenant_id = g.tenant_id
                LEFT JOIN hr.employee e
                  ON e.employee_id = g.employee_id
                 AND e.tenant_id = g.tenant_id
                WHERE g.verification_code = @VerificationCode
                  AND g.is_active
                  AND upper(g.status) = 'ISSUED'
                LIMIT 1;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<VerifyResponse>(
                       new CommandDefinition(sql, new { VerificationCode = verificationCode }, cancellationToken: cancellationToken))
                   ?? new VerifyResponse(false, null, null, null, null, null);
        }
    }

    public sealed class VerifyHandler(IVerifyCertificateQuery query)
        : IRequestHandler<VerifyQuery, Result<VerifyResponse>>
    {
        public async Task<Result<VerifyResponse>> HandleAsync(
            VerifyQuery request,
            CancellationToken cancellationToken) =>
            Result<VerifyResponse>.Success(
                await query.VerifyAsync(request.VerificationCode, cancellationToken));
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/documents/certificates",
                async (
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }
                    return (await mediator.SendAsync<DashboardQuery, Result<DashboardResponse>>(
                        new DashboardQuery(resolvedTenantId.Value), cancellationToken)).ToHttpResult();
                })
            .WithName("GetCertificateOperations")
            .WithTags("Documents")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);

        endpoints.MapPost(
                "/api/documents/certificate-templates",
                async (
                    CreateTemplateRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }
                    return (await mediator.SendAsync<CreateTemplateRequest, Result<CreateTemplateResponse>>(
                        request with { TenantId = tenantId.Value }, cancellationToken)).ToHttpResult();
                })
            .WithName("CreateCertificateTemplate")
            .WithTags("Documents")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);

        endpoints.MapPost(
                "/api/documents/certificates/issue",
                async (
                    IssueRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }
                    return (await mediator.SendAsync<IssueRequest, Result<IssueResponse>>(
                        request with { TenantId = tenantId.Value }, cancellationToken)).ToHttpResult();
                })
            .WithName("IssueCertificate")
            .WithTags("Documents")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);

        endpoints.MapPut(
                "/api/documents/certificates/{documentId:guid}/approve",
                async (
                    Guid documentId,
                    Guid? tenantId,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var resolvedTenantId = tenantScope.Resolve(tenantId);
                    if (!resolvedTenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }
                    return (await mediator.SendAsync<ApproveRequest, Result<ApproveResponse>>(
                        new ApproveRequest(resolvedTenantId.Value, documentId), cancellationToken)).ToHttpResult();
                })
            .WithName("ApproveCertificate")
            .WithTags("Documents")
            .RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);

        endpoints.MapGet(
                "/api/documents/certificates/verify/{verificationCode}",
                async (
                    string verificationCode,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(verificationCode) || verificationCode.Length > 120)
                    {
                        return Results.BadRequest(new { message = "Invalid verification code." });
                    }
                    return (await mediator.SendAsync<VerifyQuery, Result<VerifyResponse>>(
                        new VerifyQuery(verificationCode.Trim()), cancellationToken)).ToHttpResult();
                })
            .WithName("VerifyCertificate")
            .WithTags("Documents")
            .AllowAnonymous();

        return endpoints;
    }
}
