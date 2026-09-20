using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Library.Features.Operations;

/// <summary>
/// Operational library circulation workflow built on the canonical
/// library.book, library.book_copy and library.book_loan tables.
/// </summary>
public static class LibraryOperations
{
    public sealed record CatalogueItem(
        Guid BookId,
        string Title,
        string? Isbn,
        string? Author,
        string? Publisher,
        long TotalCopies,
        long AvailableCopies);

    public sealed record LoanItem(
        Guid LoanId,
        Guid BookCopyId,
        string Barcode,
        string Title,
        Guid? StudentId,
        Guid? EmployeeId,
        string BorrowerName,
        DateTimeOffset IssuedAt,
        DateTimeOffset DueAt,
        DateTimeOffset? ReturnedAt,
        bool IsOverdue);

    public sealed record DashboardQuery(Guid TenantId) : IRequest<Result<DashboardResponse>>;
    public sealed record DashboardResponse(
        IReadOnlyList<CatalogueItem> Catalogue,
        IReadOnlyList<LoanItem> CurrentLoans,
        IReadOnlyList<LoanItem> RecentReturns);

    public interface ILibraryDashboardQuery
    {
        Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class LibraryDashboardQuery(IDbConnectionFactory connectionFactory)
        : ILibraryDashboardQuery
    {
        public async Task<DashboardResponse> GetAsync(
            Guid tenantId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    b.book_id AS "BookId",
                    b.title AS "Title",
                    b.isbn AS "Isbn",
                    b.author_text AS "Author",
                    b.publisher_text AS "Publisher",
                    count(c.book_copy_id) FILTER (WHERE c.is_active) AS "TotalCopies",
                    count(c.book_copy_id) FILTER (WHERE c.is_active AND upper(c.status) = 'AVAILABLE') AS "AvailableCopies"
                FROM library.book b
                LEFT JOIN library.book_copy c
                  ON c.book_id = b.book_id
                 AND c.tenant_id = b.tenant_id
                 AND (@CampusId IS NULL OR c.campus_id = @CampusId)
                WHERE b.tenant_id = @TenantId
                  AND b.is_active
                GROUP BY b.book_id, b.title, b.isbn, b.author_text, b.publisher_text
                ORDER BY b.title;

                SELECT
                    l.book_loan_id AS "LoanId",
                    l.book_copy_id AS "BookCopyId",
                    c.barcode AS "Barcode",
                    b.title AS "Title",
                    l.student_id AS "StudentId",
                    l.employee_id AS "EmployeeId",
                    CASE
                        WHEN s.student_id IS NOT NULL THEN trim(s.first_name || ' ' || coalesce(s.last_name, ''))
                        WHEN e.employee_id IS NOT NULL THEN trim(e.first_name || ' ' || coalesce(e.last_name, ''))
                        ELSE 'Unknown borrower'
                    END AS "BorrowerName",
                    l.issued_at AS "IssuedAt",
                    l.due_at AS "DueAt",
                    l.returned_at AS "ReturnedAt",
                    (l.returned_at IS NULL AND l.due_at < now()) AS "IsOverdue"
                FROM library.book_loan l
                JOIN library.book_copy c
                  ON c.book_copy_id = l.book_copy_id
                 AND c.tenant_id = l.tenant_id
                JOIN library.book b
                  ON b.book_id = c.book_id
                 AND b.tenant_id = l.tenant_id
                LEFT JOIN student.student s
                  ON s.student_id = l.student_id
                 AND s.tenant_id = l.tenant_id
                LEFT JOIN hr.employee e
                  ON e.employee_id = l.employee_id
                 AND e.tenant_id = l.tenant_id
                WHERE l.tenant_id = @TenantId
                  AND l.is_active
                  AND l.returned_at IS NULL
                  AND (@CampusId IS NULL OR c.campus_id = @CampusId)
                ORDER BY l.due_at, l.issued_at;

                SELECT
                    l.book_loan_id AS "LoanId",
                    l.book_copy_id AS "BookCopyId",
                    c.barcode AS "Barcode",
                    b.title AS "Title",
                    l.student_id AS "StudentId",
                    l.employee_id AS "EmployeeId",
                    CASE
                        WHEN s.student_id IS NOT NULL THEN trim(s.first_name || ' ' || coalesce(s.last_name, ''))
                        WHEN e.employee_id IS NOT NULL THEN trim(e.first_name || ' ' || coalesce(e.last_name, ''))
                        ELSE 'Unknown borrower'
                    END AS "BorrowerName",
                    l.issued_at AS "IssuedAt",
                    l.due_at AS "DueAt",
                    l.returned_at AS "ReturnedAt",
                    FALSE AS "IsOverdue"
                FROM library.book_loan l
                JOIN library.book_copy c
                  ON c.book_copy_id = l.book_copy_id
                 AND c.tenant_id = l.tenant_id
                JOIN library.book b
                  ON b.book_id = c.book_id
                 AND b.tenant_id = l.tenant_id
                LEFT JOIN student.student s
                  ON s.student_id = l.student_id
                 AND s.tenant_id = l.tenant_id
                LEFT JOIN hr.employee e
                  ON e.employee_id = l.employee_id
                 AND e.tenant_id = l.tenant_id
                WHERE l.tenant_id = @TenantId
                  AND l.is_active
                  AND l.returned_at IS NOT NULL
                  AND (@CampusId IS NULL OR c.campus_id = @CampusId)
                ORDER BY l.returned_at DESC
                LIMIT 100;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            using var grid = await connection.QueryMultipleAsync(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId },
                    cancellationToken: cancellationToken));

            return new DashboardResponse(
                (await grid.ReadAsync<CatalogueItem>()).AsList(),
                (await grid.ReadAsync<LoanItem>()).AsList(),
                (await grid.ReadAsync<LoanItem>()).AsList());
        }
    }

    public sealed class DashboardHandler(
        ILibraryDashboardQuery query,
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

    public sealed record CreateBookRequest(
        Guid TenantId,
        Guid CampusId,
        string Title,
        string? Isbn,
        string? Author,
        string? Publisher,
        int CopyCount = 1) : IRequest<Result<CreateBookResponse>>;

    public sealed record CreateBookResponse(Guid BookId, int CopyCount);

    public sealed class CreateBookValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.CampusId).NotEmpty();
            RuleFor(request => request.Title).NotEmpty().MaximumLength(250);
            RuleFor(request => request.Isbn).MaximumLength(30);
            RuleFor(request => request.Author).MaximumLength(250);
            RuleFor(request => request.Publisher).MaximumLength(250);
            RuleFor(request => request.CopyCount).InclusiveBetween(1, 500);
        }
    }

    public interface ICreateLibraryBookQuery
    {
        Task<bool> CampusExistsAsync(
            Guid tenantId,
            Guid campusId,
            Guid? scopedCampusId,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateLibraryBookQuery(IDbConnectionFactory connectionFactory)
        : ICreateLibraryBookQuery
    {
        public async Task<bool> CampusExistsAsync(
            Guid tenantId,
            Guid campusId,
            Guid? scopedCampusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM org.campus
                    WHERE tenant_id = @TenantId
                      AND campus_id = @CampusId
                      AND is_active
                      AND (@ScopedCampusId IS NULL OR campus_id = @ScopedCampusId)
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, CampusId = campusId, ScopedCampusId = scopedCampusId },
                    cancellationToken: cancellationToken));
        }
    }

    public interface ICreateLibraryBookCommand
    {
        Task<CreateBookResponse> CreateAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateLibraryBookCommand(ILibraryDbContext dbContext)
        : ICreateLibraryBookCommand
    {
        public async Task<CreateBookResponse> CreateAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken)
        {
            var bookId = Guid.NewGuid();
            var bookCode = $"BOOK-{bookId:N}"[..18].ToUpperInvariant();
            var normalizedTitle = request.Title.Trim();
            var normalizedIsbn = string.IsNullOrWhiteSpace(request.Isbn) ? null : request.Isbn.Trim();
            var normalizedAuthor = string.IsNullOrWhiteSpace(request.Author) ? null : request.Author.Trim();
            var normalizedPublisher = string.IsNullOrWhiteSpace(request.Publisher) ? null : request.Publisher.Trim();

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO library.book
                (
                    book_id,
                    tenant_id,
                    isbn,
                    title,
                    author_text,
                    publisher_text,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {bookId},
                    {request.TenantId},
                    {normalizedIsbn},
                    {normalizedTitle},
                    {normalizedAuthor},
                    {normalizedPublisher},
                    {bookCode},
                    {normalizedTitle},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);

            for (var index = 1; index <= request.CopyCount; index++)
            {
                var copyId = Guid.NewGuid();
                var barcode = $"{bookCode}-{index:000}";
                await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                    INSERT INTO library.book_copy
                    (
                        book_copy_id,
                        tenant_id,
                        book_id,
                        campus_id,
                        barcode,
                        status,
                        code,
                        name,
                        is_active,
                        created_at,
                        row_version
                    )
                    VALUES
                    (
                        {copyId},
                        {request.TenantId},
                        {bookId},
                        {request.CampusId},
                        {barcode},
                        {"AVAILABLE"},
                        {barcode},
                        {normalizedTitle},
                        TRUE,
                        now(),
                        decode(md5(random()::text || clock_timestamp()::text), 'hex')
                    );
                    """, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return new CreateBookResponse(bookId, request.CopyCount);
        }
    }

    public sealed class CreateBookHandler(
        ICreateLibraryBookQuery query,
        ICreateLibraryBookCommand command,
        ICurrentUser currentUser) : IRequestHandler<CreateBookRequest, Result<CreateBookResponse>>
    {
        public async Task<Result<CreateBookResponse>> HandleAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken)
        {
            if (!await query.CampusExistsAsync(
                    request.TenantId,
                    request.CampusId,
                    currentUser.BranchId,
                    cancellationToken))
            {
                return Result<CreateBookResponse>.Failure(Error.NotFound("Campus not found."));
            }

            return Result<CreateBookResponse>.Success(
                await command.CreateAsync(request, cancellationToken));
        }
    }

    public sealed record IssueLoanRequest(
        Guid TenantId,
        Guid BookId,
        Guid? StudentId,
        Guid? EmployeeId,
        DateTimeOffset DueAt) : IRequest<Result<IssueLoanResponse>>;

    public sealed record IssueLoanResponse(Guid LoanId, Guid BookCopyId, string Barcode);

    public sealed class IssueLoanValidator : AbstractValidator<IssueLoanRequest>
    {
        public IssueLoanValidator()
        {
            RuleFor(request => request.TenantId).NotEmpty();
            RuleFor(request => request.BookId).NotEmpty();
            RuleFor(request => request)
                .Must(request => request.StudentId.HasValue ^ request.EmployeeId.HasValue)
                .WithMessage("Exactly one borrower is required: student or employee.");
            RuleFor(request => request.DueAt)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage("Due date must be in the future.");
        }
    }

    public sealed record AvailableCopy(Guid BookCopyId, Guid CampusId, string Barcode);

    public interface IIssueLibraryLoanQuery
    {
        Task<AvailableCopy?> FindAvailableCopyAsync(
            IssueLoanRequest request,
            Guid? campusId,
            CancellationToken cancellationToken);

        Task<bool> BorrowerExistsAsync(
            IssueLoanRequest request,
            Guid campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class IssueLibraryLoanQuery(IDbConnectionFactory connectionFactory)
        : IIssueLibraryLoanQuery
    {
        public async Task<AvailableCopy?> FindAvailableCopyAsync(
            IssueLoanRequest request,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    c.book_copy_id AS "BookCopyId",
                    c.campus_id AS "CampusId",
                    c.barcode AS "Barcode"
                FROM library.book_copy c
                JOIN library.book b
                  ON b.book_id = c.book_id
                 AND b.tenant_id = c.tenant_id
                WHERE c.tenant_id = @TenantId
                  AND c.book_id = @BookId
                  AND c.is_active
                  AND b.is_active
                  AND upper(c.status) = 'AVAILABLE'
                  AND (@CampusId IS NULL OR c.campus_id = @CampusId)
                ORDER BY c.barcode
                LIMIT 1;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<AvailableCopy>(
                new CommandDefinition(
                    sql,
                    new { request.TenantId, request.BookId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }

        public async Task<bool> BorrowerExistsAsync(
            IssueLoanRequest request,
            Guid campusId,
            CancellationToken cancellationToken)
        {
            const string studentSql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM student.student
                    WHERE tenant_id = @TenantId
                      AND student_id = @StudentId
                      AND branch_id = @CampusId
                      AND is_active
                      AND upper(status) = 'ACTIVE'
                );
                """;

            const string employeeSql = """
                SELECT EXISTS (
                    SELECT 1
                    FROM hr.employee
                    WHERE tenant_id = @TenantId
                      AND employee_id = @EmployeeId
                      AND branch_id = @CampusId
                      AND is_active
                      AND upper(status) IN ('ACTIVE', 'APPROVED')
                );
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var command = request.StudentId.HasValue
                ? new CommandDefinition(
                    studentSql,
                    new { request.TenantId, request.StudentId, CampusId = campusId },
                    cancellationToken: cancellationToken)
                : new CommandDefinition(
                    employeeSql,
                    new { request.TenantId, request.EmployeeId, CampusId = campusId },
                    cancellationToken: cancellationToken);

            return await connection.ExecuteScalarAsync<bool>(command);
        }
    }

    public interface IIssueLibraryLoanCommand
    {
        Task<Result<IssueLoanResponse>> IssueAsync(
            IssueLoanRequest request,
            AvailableCopy copy,
            CancellationToken cancellationToken);
    }

    internal sealed class IssueLibraryLoanCommand(ILibraryDbContext dbContext)
        : IIssueLibraryLoanCommand
    {
        public async Task<Result<IssueLoanResponse>> IssueAsync(
            IssueLoanRequest request,
            AvailableCopy copy,
            CancellationToken cancellationToken)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var reserved = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE library.book_copy
                SET status = {"LOANED"},
                    updated_at = now(),
                    row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex')
                WHERE tenant_id = {request.TenantId}
                  AND book_copy_id = {copy.BookCopyId}
                  AND is_active
                  AND upper(status) = {"AVAILABLE"};
                """, cancellationToken);

            if (reserved != 1)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<IssueLoanResponse>.Failure(
                    Error.Conflict("The selected copy is no longer available. Refresh and try again."));
            }

            var loanId = Guid.NewGuid();
            var code = $"LOAN-{loanId:N}"[..18].ToUpperInvariant();
            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                INSERT INTO library.book_loan
                (
                    book_loan_id,
                    tenant_id,
                    book_copy_id,
                    student_id,
                    employee_id,
                    issued_at,
                    due_at,
                    code,
                    name,
                    is_active,
                    created_at,
                    row_version
                )
                VALUES
                (
                    {loanId},
                    {request.TenantId},
                    {copy.BookCopyId},
                    {request.StudentId},
                    {request.EmployeeId},
                    {DateTimeOffset.UtcNow},
                    {request.DueAt},
                    {code},
                    {"Library Loan"},
                    TRUE,
                    now(),
                    decode(md5(random()::text || clock_timestamp()::text), 'hex')
                );
                """, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return Result<IssueLoanResponse>.Success(
                new IssueLoanResponse(loanId, copy.BookCopyId, copy.Barcode));
        }
    }

    public sealed class IssueLoanHandler(
        IIssueLibraryLoanQuery query,
        IIssueLibraryLoanCommand command,
        ICurrentUser currentUser) : IRequestHandler<IssueLoanRequest, Result<IssueLoanResponse>>
    {
        public async Task<Result<IssueLoanResponse>> HandleAsync(
            IssueLoanRequest request,
            CancellationToken cancellationToken)
        {
            var copy = await query.FindAvailableCopyAsync(
                request,
                currentUser.BranchId,
                cancellationToken);
            if (copy is null)
            {
                return Result<IssueLoanResponse>.Failure(
                    Error.Conflict("No available copy exists in the current campus."));
            }

            if (!await query.BorrowerExistsAsync(request, copy.CampusId, cancellationToken))
            {
                return Result<IssueLoanResponse>.Failure(
                    Error.Validation("Borrower must be active in the campus that owns the selected book copy."));
            }

            return await command.IssueAsync(request, copy, cancellationToken);
        }
    }

    public sealed record ReturnLoanRequest(Guid TenantId, Guid LoanId)
        : IRequest<Result<ReturnLoanResponse>>;

    public sealed record ReturnLoanResponse(Guid LoanId, DateTimeOffset ReturnedAt);

    public sealed record LoanToReturn(Guid BookCopyId, Guid CampusId, DateTimeOffset? ReturnedAt);

    public interface IReturnLibraryLoanQuery
    {
        Task<LoanToReturn?> GetAsync(
            Guid tenantId,
            Guid loanId,
            Guid? campusId,
            CancellationToken cancellationToken);
    }

    internal sealed class ReturnLibraryLoanQuery(IDbConnectionFactory connectionFactory)
        : IReturnLibraryLoanQuery
    {
        public async Task<LoanToReturn?> GetAsync(
            Guid tenantId,
            Guid loanId,
            Guid? campusId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    l.book_copy_id AS "BookCopyId",
                    c.campus_id AS "CampusId",
                    l.returned_at AS "ReturnedAt"
                FROM library.book_loan l
                JOIN library.book_copy c
                  ON c.book_copy_id = l.book_copy_id
                 AND c.tenant_id = l.tenant_id
                WHERE l.tenant_id = @TenantId
                  AND l.book_loan_id = @LoanId
                  AND l.is_active
                  AND (@CampusId IS NULL OR c.campus_id = @CampusId);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<LoanToReturn>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, LoanId = loanId, CampusId = campusId },
                    cancellationToken: cancellationToken));
        }
    }

    public interface IReturnLibraryLoanCommand
    {
        Task<Result<ReturnLoanResponse>> ReturnAsync(
            ReturnLoanRequest request,
            LoanToReturn loan,
            CancellationToken cancellationToken);
    }

    internal sealed class ReturnLibraryLoanCommand(ILibraryDbContext dbContext)
        : IReturnLibraryLoanCommand
    {
        public async Task<Result<ReturnLoanResponse>> ReturnAsync(
            ReturnLoanRequest request,
            LoanToReturn loan,
            CancellationToken cancellationToken)
        {
            if (loan.ReturnedAt.HasValue)
            {
                return Result<ReturnLoanResponse>.Failure(Error.Conflict("This loan has already been returned."));
            }

            var returnedAt = DateTimeOffset.UtcNow;
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            var updated = await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE library.book_loan
                SET returned_at = {returnedAt},
                    updated_at = now(),
                    row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex')
                WHERE tenant_id = {request.TenantId}
                  AND book_loan_id = {request.LoanId}
                  AND returned_at IS NULL
                  AND is_active;
                """, cancellationToken);

            if (updated != 1)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result<ReturnLoanResponse>.Failure(Error.Conflict("The loan was changed by another request."));
            }

            await dbContext.Database.ExecuteSqlInterpolatedAsync($"""
                UPDATE library.book_copy
                SET status = {"AVAILABLE"},
                    updated_at = now(),
                    row_version = decode(md5(random()::text || clock_timestamp()::text), 'hex')
                WHERE tenant_id = {request.TenantId}
                  AND book_copy_id = {loan.BookCopyId}
                  AND is_active;
                """, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return Result<ReturnLoanResponse>.Success(new ReturnLoanResponse(request.LoanId, returnedAt));
        }
    }

    public sealed class ReturnLoanHandler(
        IReturnLibraryLoanQuery query,
        IReturnLibraryLoanCommand command,
        ICurrentUser currentUser) : IRequestHandler<ReturnLoanRequest, Result<ReturnLoanResponse>>
    {
        public async Task<Result<ReturnLoanResponse>> HandleAsync(
            ReturnLoanRequest request,
            CancellationToken cancellationToken)
        {
            var loan = await query.GetAsync(
                request.TenantId,
                request.LoanId,
                currentUser.BranchId,
                cancellationToken);
            if (loan is null)
            {
                return Result<ReturnLoanResponse>.Failure(Error.NotFound("Library loan not found."));
            }

            return await command.ReturnAsync(request, loan, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/library/operations",
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
            .WithName("GetLibraryOperations")
            .WithTags("Library")
            .RequireAuthorization(SmartSchoolPolicies.LibraryManagement);

        endpoints.MapPost(
                "/api/library/operations/books",
                async (
                    CreateBookRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<CreateBookRequest, Result<CreateBookResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateOperationalLibraryBook")
            .WithTags("Library")
            .RequireAuthorization(SmartSchoolPolicies.LibraryManagement);

        endpoints.MapPost(
                "/api/library/operations/loans",
                async (
                    IssueLoanRequest request,
                    ITenantScope tenantScope,
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {
                    var tenantId = tenantScope.Resolve(request.TenantId);
                    if (!tenantId.HasValue)
                    {
                        return Results.BadRequest(new { message = "Select a tenant." });
                    }

                    var result = await mediator.SendAsync<IssueLoanRequest, Result<IssueLoanResponse>>(
                        request with { TenantId = tenantId.Value },
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("IssueOperationalLibraryLoan")
            .WithTags("Library")
            .RequireAuthorization(SmartSchoolPolicies.LibraryManagement);

        endpoints.MapPut(
                "/api/library/operations/loans/{loanId:guid}/return",
                async (
                    Guid loanId,
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

                    var result = await mediator.SendAsync<ReturnLoanRequest, Result<ReturnLoanResponse>>(
                        new ReturnLoanRequest(resolvedTenantId.Value, loanId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("ReturnOperationalLibraryLoan")
            .WithTags("Library")
            .RequireAuthorization(SmartSchoolPolicies.LibraryManagement);

        return endpoints;
    }
}
