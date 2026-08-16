using FluentValidation;
using SmartSchool.Modules.Payroll.Features.EmployeeCompensation;
using SmartSchool.Modules.Payroll.Features.Increment;
using SmartSchool.Modules.Payroll.Features.PayrollRun;
using SmartSchool.Modules.Payroll.Features.Payslip;
using SmartSchool.Modules.Payroll.Features.SalaryStructure;

namespace SmartSchool.Modules.Payroll;

public static class Module
{
    public static IServiceCollection AddPayrollModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateEmployeeCompensation.Handler>();
        services.AddScoped<GetEmployeeCompensationById.Handler>();
        services.AddScoped<GetEmployeeCompensationPage.Handler>();
        services.AddScoped<UpdateEmployeeCompensation.Handler>();
        services.AddScoped<DeleteEmployeeCompensation.Handler>();
        services.AddScoped<IValidator<CreateEmployeeCompensation.Request>, CreateEmployeeCompensation.Validator>();
        services.AddScoped<IValidator<UpdateEmployeeCompensation.Request>, UpdateEmployeeCompensation.Validator>();
        services.AddScoped<CreateIncrement.Handler>();
        services.AddScoped<GetIncrementById.Handler>();
        services.AddScoped<GetIncrementPage.Handler>();
        services.AddScoped<UpdateIncrement.Handler>();
        services.AddScoped<DeleteIncrement.Handler>();
        services.AddScoped<IValidator<CreateIncrement.Request>, CreateIncrement.Validator>();
        services.AddScoped<IValidator<UpdateIncrement.Request>, UpdateIncrement.Validator>();
        services.AddScoped<CreatePayrollRun.Handler>();
        services.AddScoped<GetPayrollRunById.Handler>();
        services.AddScoped<GetPayrollRunPage.Handler>();
        services.AddScoped<UpdatePayrollRun.Handler>();
        services.AddScoped<DeletePayrollRun.Handler>();
        services.AddScoped<IValidator<CreatePayrollRun.Request>, CreatePayrollRun.Validator>();
        services.AddScoped<IValidator<UpdatePayrollRun.Request>, UpdatePayrollRun.Validator>();
        services.AddScoped<CreatePayslip.Handler>();
        services.AddScoped<GetPayslipById.Handler>();
        services.AddScoped<GetPayslipPage.Handler>();
        services.AddScoped<UpdatePayslip.Handler>();
        services.AddScoped<DeletePayslip.Handler>();
        services.AddScoped<IValidator<CreatePayslip.Request>, CreatePayslip.Validator>();
        services.AddScoped<IValidator<UpdatePayslip.Request>, UpdatePayslip.Validator>();
        services.AddScoped<CreateSalaryStructure.Handler>();
        services.AddScoped<GetSalaryStructureById.Handler>();
        services.AddScoped<GetSalaryStructurePage.Handler>();
        services.AddScoped<UpdateSalaryStructure.Handler>();
        services.AddScoped<DeleteSalaryStructure.Handler>();
        services.AddScoped<IValidator<CreateSalaryStructure.Request>, CreateSalaryStructure.Validator>();
        services.AddScoped<IValidator<UpdateSalaryStructure.Request>, UpdateSalaryStructure.Validator>();

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
