namespace SmartSchool.Application.Identity;

public interface IIdentityAccountService
{
	Task<Guid> CreateAccountAsync(
		Guid tenantId, Guid businessEntityId, string accountType,
		string email, string firstName, string lastName,
		IReadOnlyCollection<string> roles, CancellationToken cancellationToken);

	Task DeleteAccountAsync(Guid userId, CancellationToken cancellationToken);
	Task DeactivateAccountAsync(Guid userId, CancellationToken cancellationToken);
}
