using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.Operations;

/// <summary>
/// Operational student billing workflow built on the canonical invoice,
/// payment and payment-allocation tables.
/// </summary>
public static class FinanceOperations
{
    public sealed record InvoiceItem(
        Guid InvoiceId,
        Guid StudentId,
        string StudentName,
        string InvoiceNumber,
        DateOnly InvoiceDate,
        DateOnly? DueDate,
        string Status,
        decimal TotalAmount,
        decimal BalanceAmount);

    public sealed record PaymentItem(
        Guid PaymentId,
        Guid StudentId,
        string StudentName,
        string PaymentNumber,
        DateTimeOffset PaymentDate,
        decimal Amount,
        string PaymentMethod,
        string? ReferenceNo);

    public sealed record DashboardQuery(Guid TenantId) : IRequest<Result<DashboardResponse>>;
    public sealed record DashboardResponse(
        IReadOnlyList<InvoiceItem> Invoices,
        IReadOnlyList<PaymentItem> Payments,
        decimal OutstandingBalance,
        decimal CollectedThisMonth);

    public interface IFinanceDashboardQuery
    {
        Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class FinanceDashboardQuery(IDbConnectionFactory connectionFactory)
        : IFinanceDashboardQuery
    {
        public async Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    i.student_invoice_id AS "InvoiceId",
                    i.student_id AS "StudentId",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    i.invoice_number AS "InvoiceNumber",
                    i.invoice_date AS "InvoiceDate",
                    i.due_date AS "DueDate",
                    i.status AS "Status",
                    i.total_amount AS "TotalAmount",
                    i.balance_amount AS "BalanceAmount"
                FROM finance.student_invoice i
                JOIN student.student s
                  ON s.student_id = i.student_id
                 AND s.tenant_id = i.tenant_id
                WHERE i.tenant_id = @TenantId
                  AND i.is_active
                  AND (@CampusId IS NULL OR s.branch_id = @CampusId)
                ORDER BY i.invoice_date DESC, i.created_at DESC
                LIMIT 500;

                SELECT
                    p.student_payment_id AS "PaymentId",
                    p.student_id AS "StudentId",
                    trim(s.first_name || ' ' || coalesce(s.last_name, '')) AS "StudentName",
                    p.payment_number AS "PaymentNumber",
                    p.payment_date AS "PaymentDate",
                    p.amount AS "Amount",
                    p.payment_method AS "PaymentMethod",
                    p.reference_no AS "ReferenceNo"
                FROM finance.student_payment p
                JOIN student.student s
                  ON s.student_id = p.student_id
                 AND s.tenant_id = p.tenant_id
                WHERE p.tenant_id = @TenantId
                  AND p.is_active
                  AND (@CampusId IS NULL OR s.branch_id = @CampusId)
                ORDER BY p.payment_date DESC
                LIMIT 500;

                SELECT coalesce(sum(i.balance_amount), 0)
                FROM finance.student_invoice i
                JOIN student.student s
                  ON s.student_id = i.student_id
                 AND s.tenant_id = i.tenant_id
                WHERE i.tenant_id = @TenantId
                  AND i.is_active
                  AND i.balance_amount > 0
                  AND (@CampusId IS NULL OR s.branch_id = @CampusId);

                SELECT coalesce(sum(p.amount), 0)
                FROM finance.student_payment p
                JOIN student.student s
                  ON s.student_id = p.student_id
                 AND s.tenant_id = p.tenant_id
                WHERE p.tenant_id = @TenantId
                  AND p.is_active
                  AND date_trunc('month', p.payment_date) = date_trunc('month', now())
                  AND (@CampusId IS NULL OR s.branch_id = @CampusId);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId },
                    cancellationToken: cancellationToken));

            return new DashboardResponse(
                (await grid.ReadAsync<InvoiceItem>()).AsList(),
                (await grid.ReadAsync<PaymentItem>()).AsList(),
                await grid.ReadSingleAsync<decimal>(),
                await grid.ReadSingleAsync<decimal>());
        }
    }

    public sealed class DashboardHandler(
        IFinanceDashboardQuery query,
        ICurrentUser currentUser) : IRequestHandler<DashboardQuery, Result<DashboardResponse>>
    {
        public async Task<Result<DashboardResponse>> HandleAsync(
            DashboardQuery request,
            CancellationToken cancellationToken)
        {
            var response = await query.GetAsync(
                request.TenantId,
                currentUser.BranchId,
                cancellationToken);
            return Result<DashboardResponse>.Success(response);
        }
    }

    public sealed record CreateInvoiceRequest(
        Guid TenantId,
        Guid StudentId,
        Guid? AcademicYearId,
        DateOnly InvoiceDate,
        DateOnly? DueDate,
        decimal TotalAmount,
        string? Description) : IRequest<Result<CreateInvoiceResponse>>;

    public sealed record CreateInvoiceResponse(
        Guid InvoiceId,
        string InvoiceNumber,
        decimal TotalAmount,
        decimal BalanceAmount,
        string Status);

    public sealed class CreateInvoiceValidator : AbstractValidator<CreateInvoiceRequest>
    {
        public CreateInvoiceValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.StudentId).NotEmpty();
            RuleFor(request => request.TotalAmount).GreaterThan(0);
            RuleFor(request => request.Description).MaximumLength(1000);
            RuleFor(request => request)
                .Must(request => !request.DueDate.HasValue || request.DueDate.Value >= request.InvoiceDate)
                .WithMessage("Due date cannot be before invoice date.");
        }
    }

    public interface ICreateOperationalInvoiceQuery
    {
        Task<bool> StudentExistsAsync(
            CreateInvoiceRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);

        Task<bool> AcademicYearExistsAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateOperationalInvoiceQuery(IDbConnectionFactory connectionFactory)
        : ICreateOperationalInvoiceQuery
    {
        public async Task<bool> StudentExistsAsync(
            CreateInvoiceRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM student.student
                    WHERE tenant_id = @TenantId
                      AND student_id = @StudentId
                      AND is_active
                      AND upper(status) = 'ACTIVE'
                      AND (@CampusId IS NULL OR branch_id = @CampusId)
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { request.TenantId, request.StudentId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }

        public async Task<bool> AcademicYearExistsAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken)
        {
            if (!request.AcademicYearId.HasValue)
            {
                return true;
            }

            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM academic.academic_year
                    WHERE tenant_id = @TenantId
                      AND academic_year_id = @AcademicYearId
                      AND is_active
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { request.TenantId, request.AcademicYearId },
                    cancellationToken: cancellationToken));
        }
    }

    public interface ICreateOperationalInvoiceCommand
    {
        Task<CreateInvoiceResponse> CreateAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateOperationalInvoiceCommand(IFinanceDbContext dbContext)
        : ICreateOperationalInvoiceCommand
    {
        public async Task<CreateInvoiceResponse> CreateAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken)
        {
            var invoiceId = Guid.NewGuid();
            var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{invoiceId:N}"[..26].ToUpperInvariant();
            var description = string.IsNullOrWhiteSpace(request.Description)
                ? "Student invoice"
                : request.Description.Trim();

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO finance.student_invoice
                (
                    student_invoice_id,
                    tenant_id,
                    student_id,
                    academic_year_id,
                    invoice_number,
                    invoice_date,
                    due_date,
                    status,
                    total_amount,
                    balance_amount,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {invoiceId},
                    {request.TenantId},
                    {request.StudentId},
                    {request.AcademicYearId},
                    {invoiceNumber},
                    {request.InvoiceDate},
                    {request.DueDate},
                    {"OPEN"},
                    {request.TotalAmount},
                    {request.TotalAmount},
                    {invoiceNumber},
                    {description},
                    TRUE,
                    now(),
                    gen_random_bytes(8)
                );
                """, cancellationToken);

            return new CreateInvoiceResponse(
                invoiceId,
                invoiceNumber,
                request.TotalAmount,
                request.TotalAmount,
                "OPEN");
        }
    }

    public sealed class CreateInvoiceHandler(
        ICreateOperationalInvoiceQuery query,
        ICreateOperationalInvoiceCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreateInvoiceRequest, Result<CreateInvoiceResponse>>
    {
        public async Task<Result<CreateInvoiceResponse>> HandleAsync(
            CreateInvoiceRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.StudentExistsAsync(request, currentUser.BranchId, cancellationToken))
            {
                return Result<CreateInvoiceResponse>.Failure(Error.NotFound("Student not found in the current scope."));
            }

            if (!await query.AcademicYearExistsAsync(request, cancellationToken))
            {
                return Result<CreateInvoiceResponse>.Failure(Error.NotFound("Academic year not found."));
            }

            return Result<CreateInvoiceResponse>.Success(
                await command.CreateAsync(request, cancellationToken));
        }
    }

    public sealed record PostPaymentRequest(
        Guid TenantId,
        Guid InvoiceId,
        decimal Amount,
        string PaymentMethod,
        string? ReferenceNo) : IRequest<Result<PostPaymentResponse>>;

    public sealed record PostPaymentResponse(
        Guid PaymentId,
        string PaymentNumber,
        decimal AppliedAmount,
        decimal RemainingBalance,
        string InvoiceStatus);

    public sealed class PostPaymentValidator : AbstractValidator<PostPaymentRequest>
    {
        private static readonly string[] AllowedMethods =
        [
            "CASH",
            "CARD",
            "BANK_TRANSFER",
            "CHEQUE",
            "ONLINE",
            "OTHER"
        ];

        public PostPaymentValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.InvoiceId).NotEmpty();
            RuleFor(request => request.Amount).GreaterThan(0);
            RuleFor(request => request.PaymentMethod)
                .NotEmpty()
                .Must(value => AllowedMethods.Contains(value.Trim().ToUpperInvariant(), StringComparer.Ordinal))
                .WithMessage("Unsupported payment method.");
            RuleFor(request => request.ReferenceNo).MaximumLength(150);
        }
    }

    public sealed record InvoiceForPayment(
        Guid StudentId,
        decimal BalanceAmount,
        Guid CampusId,
        string Status);

    public interface IPostStudentPaymentQuery
    {
        Task<InvoiceForPayment?> GetInvoiceAsync(
            Guid tenantId,
            Guid invoiceId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class PostStudentPaymentQuery(IDbConnectionFactory connectionFactory)
        : IPostStudentPaymentQuery
    {
        public async Task<InvoiceForPayment?> GetInvoiceAsync(
            Guid tenantId,
            Guid invoiceId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    i.student_id AS "StudentId",
                    i.balance_amount AS "BalanceAmount",
                    s.branch_id AS "CampusId",
                    i.status AS "Status"
                FROM finance.student_invoice i
                JOIN student.student s
                  ON s.student_id = i.student_id
                 AND s.tenant_id = i.tenant_id
                WHERE i.tenant_id = @TenantId
                  AND i.student_invoice_id = @InvoiceId
                  AND i.is_active
                  AND s.is_active
                  AND (@CampusId IS NULL OR s.branch_id = @CampusId);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<InvoiceForPayment>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, InvoiceId = invoiceId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }
    }

    public interface IPostStudentPaymentCommand
    {
        Task<Result<PostPaymentResponse>> PostAsync(
            PostPaymentRequest request,
            InvoiceForPayment invoice,
            CancellationToken cancellationToken);
    }

    internal sealed class PostStudentPaymentCommand(IFinanceDbContext dbContext)
        : IPostStudentPaymentCommand
    {
        public async Task<Result<PostPaymentResponse>> PostAsync(
            PostPaymentRequest request,
            InvoiceForPayment invoice,
            CancellationToken cancellationToken)
        {
            if (invoice.BalanceAmount <= 0)
            {
                return Result<PostPaymentResponse>.Failure(Error.Conflict("Invoice is already fully paid."));
            }

            if (request.Amount > invoice.BalanceAmount)
            {
                return Result<PostPaymentResponse>.Failure(
                    Error.Validation("Payment amount cannot exceed the outstanding invoice balance."));
            }

            var remaining = invoice.BalanceAmount - request.Amount;
            var status = remaining == 0 ? "PAID" : "PARTIALLY_PAID";
            var paymentId = Guid.NewGuid();
            var allocationId = Guid.NewGuid();
            var paymentNumber = $"PAY-{DateTime.UtcNow:yyyyMMdd}-{paymentId:N}"[..26].ToUpperInvariant();
            var paymentMethod = request.PaymentMethod.Trim().ToUpperInvariant();
            var reference = string.IsNullOrWhiteSpace(request.ReferenceNo) ? null : request.ReferenceNo.Trim();

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var invoiceUpdated = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE finance.student_invoice
                SET balance_amount = balance_amount - {request.Amount},
                    status = {status},
                    updated_at = now(),
                    row_version = gen_random_bytes(8)
                WHERE tenant_id = {request.TenantId}
                  AND student_invoice_id = {request.InvoiceId}
                  AND is_active
                  AND balance_amount >= {request.Amount};
                """, cancellationToken);

            if (invoiceUpdated != 1)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<PostPaymentResponse>.Failure(
                    Error.Conflict("Invoice balance changed. Refresh and try again."));
            }

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO finance.student_payment
                (
                    student_payment_id,
                    tenant_id,
                    student_id,
                    payment_number,
                    payment_date,
                    amount,
                    payment_method,
                    reference_no,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {paymentId},
                    {request.TenantId},
                    {invoice.StudentId},
                    {paymentNumber},
                    {DateTimeOffset.UtcNow},
                    {request.Amount},
                    {paymentMethod},
                    {reference},
                    {paymentNumber},
                    {"Student payment"},
                    TRUE,
                    now(),
                    gen_random_bytes(8)
                );
                """, cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO finance.payment_allocation
                (
                    payment_allocation_id,
                    student_payment_id,
                    student_invoice_id,
                    amount
                )
                VALUES
                (
                    {allocationId},
                    {paymentId},
                    {request.InvoiceId},
                    {request.Amount}
                );
                """, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return Result<PostPaymentResponse>.Success(
                new PostPaymentResponse(paymentId, paymentNumber, request.Amount, remaining, status));
        }
    }

    public sealed class PostPaymentHandler(
        IPostStudentPaymentQuery query,
        IPostStudentPaymentCommand command,
        ICurrentUser currentUser) : IRequestHandler<PostPaymentRequest, Result<PostPaymentResponse>>
    {
        public async Task<Result<PostPaymentResponse>> HandleAsync(
            PostPaymentRequest request,
            CancellationToken cancellationToken)
        {
            var invoice = await query.GetInvoiceAsync(
                request.TenantId,
                request.InvoiceId,
                currentUser.BranchId,
                cancellationToken);
            if (invoice is null)
            {
                return Result<PostPaymentResponse>.Failure(Error.NotFound("Invoice not found in the current scope."));
            }

            return await command.PostAsync(request, invoice, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/finance/operations",
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

                    var result = await mediator.SendAsync<DashboardQuery, Result<DashboardResponse>>(
                        new DashboardQuery(resolvedTenantId.Value),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetFinanceOperations")
            .WithTags("Finance")
            .RequireAuthorization(SmartSchoolPolicies.FinanceManagement);

        endpoints.MapPost(
                "/api/finance/operations/invoices",
                async (
                    CreateInvoiceRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<CreateInvoiceRequest, Result<CreateInvoiceResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateOperationalStudentInvoice")
            .WithTags("Finance")
            .RequireAuthorization(SmartSchoolPolicies.FinanceTransactions);

        endpoints.MapPost(
                "/api/finance/operations/payments",
                async (
                    PostPaymentRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<PostPaymentRequest, Result<PostPaymentResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("PostOperationalStudentPayment")
            .WithTags("Finance")
            .RequireAuthorization(SmartSchoolPolicies.FinanceTransactions);

        return endpoints;
    }
}
