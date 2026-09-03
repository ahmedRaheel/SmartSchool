using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Organization.Features.Campus;

public sealed class BranchPolicyQuery(IDbConnectionFactory connectionFactory) : IBranchPolicyQuery
{
    public Task<IReadOnlyCollection<LookupItem>> GetGenderTypesAsync(CancellationToken cancellationToken) =>
        GetLookupsAsync("SELECT branch_gender_type_id AS Id, code AS Code, name AS Name FROM reference.branch_gender_type WHERE is_active = TRUE ORDER BY sort_order, name;", cancellationToken);

    public Task<IReadOnlyCollection<LookupItem>> GetEducationLevelsAsync(CancellationToken cancellationToken) =>
		GetLookupsAsync("SELECT education_level_id AS Id, code AS Code, name AS Name FROM reference.education_level WHERE is_active = TRUE ORDER BY sort_order, name;", cancellationToken);

	public async Task<BranchPolicy?> GetBranchPolicyAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken)
    {
        const string headerSql = """
            SELECT c.branch_gender_type_id AS BranchGenderTypeId, g.code AS GenderCode
            FROM org.campus c
            INNER JOIN reference.branch_gender_type g ON g.branch_gender_type_id = c.branch_gender_type_id
            WHERE c.tenant_id = @TenantId AND c.campus_id = @BranchId AND c.is_active = TRUE;
            """;
        const string levelsSql = """
            SELECT l.education_level_id AS Id, l.code AS Code, l.name AS Name
            FROM org.campus_education_level b
            INNER JOIN reference.education_level l ON l.education_level_id = b.education_level_id
            WHERE b.tenant_id = @TenantId AND b.campus_id = @BranchId AND l.is_active = TRUE
            ORDER BY l.sort_order, l.name;
            """;
		await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
		var (genderTypeId, genderCode) = await connection.QuerySingleOrDefaultAsync<(Guid BranchGenderTypeId, string GenderCode)>(new CommandDefinition(headerSql, new
		{
			TenantId = tenantId,
			BranchId = branchId
		}, cancellationToken: cancellationToken));
        if (genderTypeId == Guid.Empty) return null;
        var levels = (await connection.QueryAsync<LookupItem>(new CommandDefinition(levelsSql, new
		{
            TenantId = tenantId,
			BranchId = branchId
		}, cancellationToken: cancellationToken))).AsList();
        return new BranchPolicy(genderTypeId, genderCode, levels);
	}

	private async Task<IReadOnlyCollection<LookupItem>> GetLookupsAsync(string sql, CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<LookupItem>(new CommandDefinition(sql, cancellationToken: cancellationToken))).AsList();
    }
}
