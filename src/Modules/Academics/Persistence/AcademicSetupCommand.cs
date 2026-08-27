using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Academics.Persistence;

public sealed class AcademicSetupCommand(
    IDbConnectionFactory connectionFactory) : IAcademicSetupCommand
{
    public async Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND school_id = @SchoolId
                  AND campus_id = @BranchId
                  AND is_active = TRUE
            );
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    SchoolId = schoolId,
                    BranchId = branchId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<AcademicSetupItem> CreateAcademicYearAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        string name,
        string code,
        DateOnly startDate,
        DateOnly endDate,
        bool isCurrent,
        CancellationToken cancellationToken)
    {
        const string clearCurrentSql = """
            UPDATE academic.academic_year
            SET is_current = FALSE
            WHERE tenant_id = @TenantId
              AND branch_id = @BranchId
              AND is_current = TRUE;
            """;

        const string insertSql = """
            INSERT INTO academic.academic_year
            (
                academic_year_id,
                tenant_id,
                school_id,
                branch_id,
                campus_id,
                code,
                name,
                start_date,
                end_date,
                is_current
            )
            VALUES
            (
                @Id,
                @TenantId,
                @SchoolId,
                @BranchId,
                @BranchId,
                @Code,
                @Name,
                @StartDate,
                @EndDate,
                @IsCurrent
            );
            """;

        var id = Guid.NewGuid();

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        var parameters = new
        {
            Id = id,
            TenantId = tenantId,
            SchoolId = schoolId,
            BranchId = branchId,
            Code = code,
            Name = name,
            StartDate = startDate,
            EndDate = endDate,
            IsCurrent = isCurrent
        };

        if (isCurrent)
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    clearCurrentSql,
                    parameters,
                    transaction,
                    cancellationToken: cancellationToken));
        }

        await connection.ExecuteAsync(
            new CommandDefinition(
                insertSql,
                parameters,
                transaction,
                cancellationToken: cancellationToken));

        await transaction.CommitAsync(cancellationToken);

        return new AcademicSetupItem(
            id,
            name,
            code,
            branchId,
            StartDate: startDate,
            EndDate: endDate,
            IsCurrent: isCurrent);
    }

    public async Task<AcademicSetupItem> CreateClassAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        string name,
        string code,
        Guid educationLevelId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO academic.class
            (
                class_id,
                tenant_id,
                school_id,
                branch_id,
                code,
                name,
                education_level_id
            )
            VALUES
            (
                @Id,
                @TenantId,
                @SchoolId,
                @BranchId,
                @Code,
                @Name,
                @EducationLevelId
            );
            """;

        var id = Guid.NewGuid();

        await ExecuteAsync(
            sql,
            new
            {
                Id = id,
                TenantId = tenantId,
                SchoolId = schoolId,
                BranchId = branchId,
                Code = code,
                Name = name,
                EducationLevelId = educationLevelId
            },
            cancellationToken);

        return new AcademicSetupItem(id, name, code, branchId, EducationLevelId: educationLevelId);
    }

    public async Task<AcademicSetupItem> CreateSectionAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        string name,
        string code,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO academic.section
            (
                section_id,
                tenant_id,
                branch_id,
                class_id,
                code,
                name
            )
            VALUES
            (
                @Id,
                @TenantId,
                @BranchId,
                @ClassId,
                @Code,
                @Name
            );
            """;

        var id = Guid.NewGuid();

        await ExecuteAsync(
            sql,
            new
            {
                Id = id,
                TenantId = tenantId,
                BranchId = branchId,
                ClassId = classId,
                Code = code,
                Name = name
            },
            cancellationToken);

        return new AcademicSetupItem(
            id,
            name,
            code,
            branchId,
            classId);
    }

    private async Task ExecuteAsync(
        string sql,
        object parameters,
        CancellationToken cancellationToken)
    {
        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                parameters,
                cancellationToken: cancellationToken));
    }
}
