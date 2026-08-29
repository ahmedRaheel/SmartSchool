using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Features.EmployeeCompensation;
using SmartSchool.Modules.Payroll.Features.PayrollRun;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Payroll.Features.Increment;
using SmartSchool.Modules.Payroll.Features.Payslip;
using SmartSchool.Modules.Payroll.Features.SalaryStructure;

namespace SmartSchool.Modules.Payroll;

public static class Module
{
	public static IServiceCollection AddPayrollModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);

        services.AddFeatureDataAccess(typeof(Module).Assembly);
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
		CreatePayrollRun.MapEndpoint(endpoints);
		GetPayrollRunById.MapEndpoint(endpoints);
		GetPayrollRunPage.MapEndpoint(endpoints);
		UpdatePayrollRun.MapEndpoint(endpoints);
		DeletePayrollRun.MapEndpoint(endpoints);

		CreateIncrement.MapEndpoint(endpoints);
		CreatePayslip.MapEndpoint(endpoints);
		CreateSalaryStructure.MapEndpoint(endpoints);
		DeleteIncrement.MapEndpoint(endpoints);
		DeletePayslip.MapEndpoint(endpoints);
		DeleteSalaryStructure.MapEndpoint(endpoints);
		GetIncrementById.MapEndpoint(endpoints);
		GetIncrementPage.MapEndpoint(endpoints);
		GetPayslipById.MapEndpoint(endpoints);
		GetPayslipPage.MapEndpoint(endpoints);
		GetSalaryStructureById.MapEndpoint(endpoints);
		GetSalaryStructurePage.MapEndpoint(endpoints);
		UpdateIncrement.MapEndpoint(endpoints);
		UpdatePayslip.MapEndpoint(endpoints);
		UpdateSalaryStructure.MapEndpoint(endpoints);

		return endpoints;
	}
}
