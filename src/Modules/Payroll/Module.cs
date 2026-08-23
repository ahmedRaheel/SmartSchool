using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Features.EmployeeCompensation;
using SmartSchool.Modules.Payroll.Features.PayrollRun;
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
		services.AddScoped<IPayrollRunQuery, PayrollRunQuery>();
		services.AddScoped<IPayrollRunCommand, PayrollRunCommand>();

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

		return endpoints;
	}
}
