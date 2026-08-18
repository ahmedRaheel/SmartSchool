using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Models;

/// <summary>
/// Stores payroll configuration for an employee without storing banking secrets.
/// </summary>
public sealed class PayrollProfileEntity : Entity
{
	public Guid EmployeeId { get; private set; }
	public decimal BasicSalary { get; private set; }
	public decimal HouseAllowance { get; private set; }
	public decimal MedicalAllowance { get; private set; }
	public decimal TransportAllowance { get; private set; }
	public decimal OtherAllowance { get; private set; }
	public decimal TaxDeduction { get; private set; }
	public decimal ProvidentFundDeduction { get; private set; }
	public decimal OtherDeduction { get; private set; }
	public string CurrencyCode { get; private set; } = "PKR";
	public string PayFrequencyCode { get; private set; } = "Monthly";
	public string? BankName { get; private set; }
	public string? AccountTitle { get; private set; }
	public string? MaskedAccountNumber { get; private set; }
	public DateOnly EffectiveFrom { get; private set; }

	private PayrollProfileEntity() { }
}
