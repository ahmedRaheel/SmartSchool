
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Features.EmployeeCompensation;
using SmartSchool.Modules.Payroll.Features.Increment;
using SmartSchool.Modules.Payroll.Features.PayrollRun;
using SmartSchool.Modules.Payroll.Features.Payslip;
using SmartSchool.Modules.Payroll.Features.SalaryStructure;
using SmartSchool.Modules.Payroll.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll;

public static class Module
{
	public static IServiceCollection AddPayrollModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IEmployeeCompensationQuery, EmployeeCompensationQuery>();
		services.AddScoped<IEmployeeCompensationCommand, EmployeeCompensationCommand>();
		services.AddScoped<IIncrementQuery, IncrementQuery>();
		services.AddScoped<IIncrementCommand, IncrementCommand>();
		services.AddScoped<IPayrollRunQuery, PayrollRunQuery>();
		services.AddScoped<IPayrollRunCommand, PayrollRunCommand>();
		services.AddScoped<IPayslipQuery, PayslipQuery>();
		services.AddScoped<IPayslipCommand, PayslipCommand>();
		services.AddScoped<ISalaryStructureQuery, SalaryStructureQuery>();
		services.AddScoped<ISalaryStructureCommand, SalaryStructureCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapPayrollEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateEmployeeCompensation.MapEndpoint(endpoints);
		GetEmployeeCompensationById.MapEndpoint(endpoints);
		GetEmployeeCompensationPage.MapEndpoint(endpoints);
		UpdateEmployeeCompensation.MapEndpoint(endpoints);
		DeleteEmployeeCompensation.MapEndpoint(endpoints);
		CreateIncrement.MapEndpoint(endpoints);
		GetIncrementById.MapEndpoint(endpoints);
		GetIncrementPage.MapEndpoint(endpoints);
		UpdateIncrement.MapEndpoint(endpoints);
		DeleteIncrement.MapEndpoint(endpoints);
		CreatePayrollRun.MapEndpoint(endpoints);
		GetPayrollRunById.MapEndpoint(endpoints);
		GetPayrollRunPage.MapEndpoint(endpoints);
		UpdatePayrollRun.MapEndpoint(endpoints);
		DeletePayrollRun.MapEndpoint(endpoints);
		CreatePayslip.MapEndpoint(endpoints);
		GetPayslipById.MapEndpoint(endpoints);
		GetPayslipPage.MapEndpoint(endpoints);
		UpdatePayslip.MapEndpoint(endpoints);
		DeletePayslip.MapEndpoint(endpoints);
		CreateSalaryStructure.MapEndpoint(endpoints);
		GetSalaryStructureById.MapEndpoint(endpoints);
		GetSalaryStructurePage.MapEndpoint(endpoints);
		UpdateSalaryStructure.MapEndpoint(endpoints);
		DeleteSalaryStructure.MapEndpoint(endpoints);

		return endpoints;
	}
}
