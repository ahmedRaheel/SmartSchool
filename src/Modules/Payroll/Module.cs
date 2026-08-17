using SmartSchool.Modules.Payroll.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Payroll.Persistence;
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
        services.AddScoped<IValidator<CreateEmployeeCompensation.Request>, CreateEmployeeCompensation.Validator>();
        services.AddScoped<IValidator<UpdateEmployeeCompensation.Request>, UpdateEmployeeCompensation.Validator>();
        services.AddScoped<IValidator<CreateIncrement.Request>, CreateIncrement.Validator>();
        services.AddScoped<IValidator<UpdateIncrement.Request>, UpdateIncrement.Validator>();
        services.AddScoped<IValidator<CreatePayrollRun.Request>, CreatePayrollRun.Validator>();
        services.AddScoped<IValidator<UpdatePayrollRun.Request>, UpdatePayrollRun.Validator>();
        services.AddScoped<IValidator<CreatePayslip.Request>, CreatePayslip.Validator>();
        services.AddScoped<IValidator<UpdatePayslip.Request>, UpdatePayslip.Validator>();
        services.AddScoped<IValidator<CreateSalaryStructure.Request>, CreateSalaryStructure.Validator>();
        services.AddScoped<IValidator<UpdateSalaryStructure.Request>, UpdateSalaryStructure.Validator>();


        services.AddScoped<IRequestHandler<CreateEmployeeCompensation.Request, Result<EmployeeCompensationResponse>>, CreateEmployeeCompensation.Handler>();
        services.AddScoped<IRequestHandler<GetEmployeeCompensationById.Query, Result<EmployeeCompensationResponse>>, GetEmployeeCompensationById.Handler>();
        services.AddScoped<IRequestHandler<GetEmployeeCompensationPage.Query, Result<PagedResult<EmployeeCompensationResponse>>>, GetEmployeeCompensationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateEmployeeCompensation.Request, Result<EmployeeCompensationResponse>>, UpdateEmployeeCompensation.Handler>();
        services.AddScoped<IRequestHandler<DeleteEmployeeCompensation.Command, Result<DeleteEmployeeCompensation.Response>>, DeleteEmployeeCompensation.Handler>();
        services.AddScoped<IRequestHandler<CreateIncrement.Request, Result<IncrementResponse>>, CreateIncrement.Handler>();
        services.AddScoped<IRequestHandler<GetIncrementById.Query, Result<IncrementResponse>>, GetIncrementById.Handler>();
        services.AddScoped<IRequestHandler<GetIncrementPage.Query, Result<PagedResult<IncrementResponse>>>, GetIncrementPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateIncrement.Request, Result<IncrementResponse>>, UpdateIncrement.Handler>();
        services.AddScoped<IRequestHandler<DeleteIncrement.Command, Result<DeleteIncrement.Response>>, DeleteIncrement.Handler>();
        services.AddScoped<IRequestHandler<CreatePayrollRun.Request, Result<PayrollRunResponse>>, CreatePayrollRun.Handler>();
        services.AddScoped<IRequestHandler<GetPayrollRunById.Query, Result<PayrollRunResponse>>, GetPayrollRunById.Handler>();
        services.AddScoped<IRequestHandler<GetPayrollRunPage.Query, Result<PagedResult<PayrollRunResponse>>>, GetPayrollRunPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePayrollRun.Request, Result<PayrollRunResponse>>, UpdatePayrollRun.Handler>();
        services.AddScoped<IRequestHandler<DeletePayrollRun.Command, Result<DeletePayrollRun.Response>>, DeletePayrollRun.Handler>();
        services.AddScoped<IRequestHandler<CreatePayslip.Request, Result<PayslipResponse>>, CreatePayslip.Handler>();
        services.AddScoped<IRequestHandler<GetPayslipById.Query, Result<PayslipResponse>>, GetPayslipById.Handler>();
        services.AddScoped<IRequestHandler<GetPayslipPage.Query, Result<PagedResult<PayslipResponse>>>, GetPayslipPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePayslip.Request, Result<PayslipResponse>>, UpdatePayslip.Handler>();
        services.AddScoped<IRequestHandler<DeletePayslip.Command, Result<DeletePayslip.Response>>, DeletePayslip.Handler>();
        services.AddScoped<IRequestHandler<CreateSalaryStructure.Request, Result<SalaryStructureResponse>>, CreateSalaryStructure.Handler>();
        services.AddScoped<IRequestHandler<GetSalaryStructureById.Query, Result<SalaryStructureResponse>>, GetSalaryStructureById.Handler>();
        services.AddScoped<IRequestHandler<GetSalaryStructurePage.Query, Result<PagedResult<SalaryStructureResponse>>>, GetSalaryStructurePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateSalaryStructure.Request, Result<SalaryStructureResponse>>, UpdateSalaryStructure.Handler>();
        services.AddScoped<IRequestHandler<DeleteSalaryStructure.Command, Result<DeleteSalaryStructure.Response>>, DeleteSalaryStructure.Handler>();

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
