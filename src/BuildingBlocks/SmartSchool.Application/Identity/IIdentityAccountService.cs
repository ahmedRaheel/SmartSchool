namespace SmartSchool.Application.Identity;

public sealed record ProvisionedAccount(
    Guid UserId,
    string Email,
    string TemporaryPassword,
    bool MustChangePassword);

public interface IIdentityAccountService
{
    Task<ProvisionedAccount> CreateAccountAsync(
        Guid tenantId,
        Guid businessEntityId,
        string accountType,
        string email,
        string firstName,
        string lastName,
        Guid? schoolId,
        Guid? branchId,
        IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken);

    Task DeleteAccountAsync(Guid userId, CancellationToken cancellationToken);
    Task DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken);
}
