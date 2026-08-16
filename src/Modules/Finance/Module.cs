using SmartSchool.Modules.Finance.Persistence;
using FluentValidation;
using SmartSchool.Modules.Finance.Features.Discount;
using SmartSchool.Modules.Finance.Features.FeeStructure;
using SmartSchool.Modules.Finance.Features.FeeType;
using SmartSchool.Modules.Finance.Features.Invoice;
using SmartSchool.Modules.Finance.Features.Payment;
using SmartSchool.Modules.Finance.Features.Scholarship;
using SmartSchool.Modules.Finance.Features.StudentFee;

namespace SmartSchool.Modules.Finance;

public static class Module
{
    public static IServiceCollection AddFinanceModule(
        this IServiceCollection services)
    {
        services.AddScoped<IDiscountQuery, DiscountQuery>();
        services.AddScoped<IDiscountCommand, DiscountCommand>();
        services.AddScoped<IFeeStructureQuery, FeeStructureQuery>();
        services.AddScoped<IFeeStructureCommand, FeeStructureCommand>();
        services.AddScoped<IFeeTypeQuery, FeeTypeQuery>();
        services.AddScoped<IFeeTypeCommand, FeeTypeCommand>();
        services.AddScoped<IInvoiceQuery, InvoiceQuery>();
        services.AddScoped<IInvoiceCommand, InvoiceCommand>();
        services.AddScoped<IPaymentQuery, PaymentQuery>();
        services.AddScoped<IPaymentCommand, PaymentCommand>();
        services.AddScoped<IScholarshipQuery, ScholarshipQuery>();
        services.AddScoped<IScholarshipCommand, ScholarshipCommand>();
        services.AddScoped<IStudentFeeQuery, StudentFeeQuery>();
        services.AddScoped<IStudentFeeCommand, StudentFeeCommand>();

        services.AddScoped<CreateDiscount.Handler>();
        services.AddScoped<GetDiscountById.Handler>();
        services.AddScoped<GetDiscountPage.Handler>();
        services.AddScoped<UpdateDiscount.Handler>();
        services.AddScoped<DeleteDiscount.Handler>();
        services.AddScoped<IValidator<CreateDiscount.Request>, CreateDiscount.Validator>();
        services.AddScoped<IValidator<UpdateDiscount.Request>, UpdateDiscount.Validator>();
        services.AddScoped<CreateFeeStructure.Handler>();
        services.AddScoped<GetFeeStructureById.Handler>();
        services.AddScoped<GetFeeStructurePage.Handler>();
        services.AddScoped<UpdateFeeStructure.Handler>();
        services.AddScoped<DeleteFeeStructure.Handler>();
        services.AddScoped<IValidator<CreateFeeStructure.Request>, CreateFeeStructure.Validator>();
        services.AddScoped<IValidator<UpdateFeeStructure.Request>, UpdateFeeStructure.Validator>();
        services.AddScoped<CreateFeeType.Handler>();
        services.AddScoped<GetFeeTypeById.Handler>();
        services.AddScoped<GetFeeTypePage.Handler>();
        services.AddScoped<UpdateFeeType.Handler>();
        services.AddScoped<DeleteFeeType.Handler>();
        services.AddScoped<IValidator<CreateFeeType.Request>, CreateFeeType.Validator>();
        services.AddScoped<IValidator<UpdateFeeType.Request>, UpdateFeeType.Validator>();
        services.AddScoped<CreateInvoice.Handler>();
        services.AddScoped<GetInvoiceById.Handler>();
        services.AddScoped<GetInvoicePage.Handler>();
        services.AddScoped<UpdateInvoice.Handler>();
        services.AddScoped<DeleteInvoice.Handler>();
        services.AddScoped<IValidator<CreateInvoice.Request>, CreateInvoice.Validator>();
        services.AddScoped<IValidator<UpdateInvoice.Request>, UpdateInvoice.Validator>();
        services.AddScoped<CreatePayment.Handler>();
        services.AddScoped<GetPaymentById.Handler>();
        services.AddScoped<GetPaymentPage.Handler>();
        services.AddScoped<UpdatePayment.Handler>();
        services.AddScoped<DeletePayment.Handler>();
        services.AddScoped<IValidator<CreatePayment.Request>, CreatePayment.Validator>();
        services.AddScoped<IValidator<UpdatePayment.Request>, UpdatePayment.Validator>();
        services.AddScoped<CreateScholarship.Handler>();
        services.AddScoped<GetScholarshipById.Handler>();
        services.AddScoped<GetScholarshipPage.Handler>();
        services.AddScoped<UpdateScholarship.Handler>();
        services.AddScoped<DeleteScholarship.Handler>();
        services.AddScoped<IValidator<CreateScholarship.Request>, CreateScholarship.Validator>();
        services.AddScoped<IValidator<UpdateScholarship.Request>, UpdateScholarship.Validator>();
        services.AddScoped<CreateStudentFee.Handler>();
        services.AddScoped<GetStudentFeeById.Handler>();
        services.AddScoped<GetStudentFeePage.Handler>();
        services.AddScoped<UpdateStudentFee.Handler>();
        services.AddScoped<DeleteStudentFee.Handler>();
        services.AddScoped<IValidator<CreateStudentFee.Request>, CreateStudentFee.Validator>();
        services.AddScoped<IValidator<UpdateStudentFee.Request>, UpdateStudentFee.Validator>();

        return services;
    }

    public static IEndpointRouteBuilder MapFinanceEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateDiscount.MapEndpoint(endpoints);
        GetDiscountById.MapEndpoint(endpoints);
        GetDiscountPage.MapEndpoint(endpoints);
        UpdateDiscount.MapEndpoint(endpoints);
        DeleteDiscount.MapEndpoint(endpoints);
        CreateFeeStructure.MapEndpoint(endpoints);
        GetFeeStructureById.MapEndpoint(endpoints);
        GetFeeStructurePage.MapEndpoint(endpoints);
        UpdateFeeStructure.MapEndpoint(endpoints);
        DeleteFeeStructure.MapEndpoint(endpoints);
        CreateFeeType.MapEndpoint(endpoints);
        GetFeeTypeById.MapEndpoint(endpoints);
        GetFeeTypePage.MapEndpoint(endpoints);
        UpdateFeeType.MapEndpoint(endpoints);
        DeleteFeeType.MapEndpoint(endpoints);
        CreateInvoice.MapEndpoint(endpoints);
        GetInvoiceById.MapEndpoint(endpoints);
        GetInvoicePage.MapEndpoint(endpoints);
        UpdateInvoice.MapEndpoint(endpoints);
        DeleteInvoice.MapEndpoint(endpoints);
        CreatePayment.MapEndpoint(endpoints);
        GetPaymentById.MapEndpoint(endpoints);
        GetPaymentPage.MapEndpoint(endpoints);
        UpdatePayment.MapEndpoint(endpoints);
        DeletePayment.MapEndpoint(endpoints);
        CreateScholarship.MapEndpoint(endpoints);
        GetScholarshipById.MapEndpoint(endpoints);
        GetScholarshipPage.MapEndpoint(endpoints);
        UpdateScholarship.MapEndpoint(endpoints);
        DeleteScholarship.MapEndpoint(endpoints);
        CreateStudentFee.MapEndpoint(endpoints);
        GetStudentFeeById.MapEndpoint(endpoints);
        GetStudentFeePage.MapEndpoint(endpoints);
        UpdateStudentFee.MapEndpoint(endpoints);
        DeleteStudentFee.MapEndpoint(endpoints);

        return endpoints;
    }
}
