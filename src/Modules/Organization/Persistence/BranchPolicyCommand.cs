using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Organization.Persistence;

public sealed class BranchPolicyCommand(IDbConnectionFactory connectionFactory) : IBranchPolicyCommand
{
    public async Task<bool> GenderTypeExistsAsync(Guid genderTypeId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM reference.branch_gender_type WHERE branch_gender_type_id=@Id AND is_active=TRUE);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { Id = genderTypeId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> EducationLevelsExistAsync(IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken)
    {
        if (educationLevelIds.Count == 0) return false;
        const string sql = "SELECT COUNT(*) FROM reference.education_level WHERE education_level_id = ANY(@Ids) AND is_active=TRUE;";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var count = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { Ids = educationLevelIds.ToArray() }, cancellationToken: cancellationToken));
        return count == educationLevelIds.Distinct().Count();
    }

    public async Task SetEducationLevelsAsync(Guid branchId, IReadOnlyCollection<Guid> educationLevelIds, CancellationToken cancellationToken)
    {
        const string deleteSql = "DELETE FROM org.branch_education_level WHERE branch_id=@BranchId;";
        const string insertSql = "INSERT INTO org.branch_education_level(branch_id, education_level_id) VALUES(@BranchId, @EducationLevelId);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        await connection.ExecuteAsync(new CommandDefinition(deleteSql, new { BranchId = branchId }, transaction, cancellationToken: cancellationToken));
        foreach (var levelId in educationLevelIds.Distinct())
            await connection.ExecuteAsync(new CommandDefinition(insertSql, new { BranchId = branchId, EducationLevelId = levelId }, transaction, cancellationToken: cancellationToken));
        await transaction.CommitAsync(cancellationToken);
    }
}
