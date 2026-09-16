using SmartSchool.Modules.Payroll.Features.Operations;
using SmartSchool.Modules.Payroll.Persistence;
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
        services.AddScoped<IPayrollDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<PayrollDbContext>());

        services.AddFeaturePersistence(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapPayrollEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        GetEmployeeCompensationById.MapEndpoint(endpoints);
        GetEmployeeCompensationByJobGradeId.MapEndpoint(endpoints);
        GetEmployeeCompensationByEmployeeId.MapEndpoint(endpoints);
        GetEmployeeCompensationPage.MapEndpoint(endpoints);
        GetPayrollRunById.MapEndpoint(endpoints);
        GetPayrollRunPage.MapEndpoint(endpoints);


        PayrollOperations.MapEndpoints(endpoints);

        return endpoints;
    }
}
