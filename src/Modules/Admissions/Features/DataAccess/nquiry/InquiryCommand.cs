using SmartSchool.Modules.Admissions.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

using SmartSchool.Modules.Admissions.Features.Inquiry;

namespace SmartSchool.Modules.Admissions.Features.DataAccess.Inquiry;

/// <summary>
/// Executes database writes for <see cref="InquiryEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InquiryCommand(IAdmissionsDbContext dbContext) : IInquiryCommand
{
	public async Task AddAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Inquiries
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Inquiries
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InquiryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Inquiries
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
