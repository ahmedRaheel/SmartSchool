using SmartSchool.Modules.Finance.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
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
        services.AddScoped<IValidator<CreateDiscount.Request>, CreateDiscount.Validator>();
        services.AddScoped<IValidator<UpdateDiscount.Request>, UpdateDiscount.Validator>();
        services.AddScoped<IValidator<CreateFeeStructure.Request>, CreateFeeStructure.Validator>();
        services.AddScoped<IValidator<UpdateFeeStructure.Request>, UpdateFeeStructure.Validator>();
        services.AddScoped<IValidator<CreateFeeType.Request>, CreateFeeType.Validator>();
        services.AddScoped<IValidator<UpdateFeeType.Request>, UpdateFeeType.Validator>();
        services.AddScoped<IValidator<CreateInvoice.Request>, CreateInvoice.Validator>();
        services.AddScoped<IValidator<UpdateInvoice.Request>, UpdateInvoice.Validator>();
        services.AddScoped<IValidator<CreatePayment.Request>, CreatePayment.Validator>();
        services.AddScoped<IValidator<UpdatePayment.Request>, UpdatePayment.Validator>();
        services.AddScoped<IValidator<CreateScholarship.Request>, CreateScholarship.Validator>();
        services.AddScoped<IValidator<UpdateScholarship.Request>, UpdateScholarship.Validator>();
        services.AddScoped<IValidator<CreateStudentFee.Request>, CreateStudentFee.Validator>();
        services.AddScoped<IValidator<UpdateStudentFee.Request>, UpdateStudentFee.Validator>();


        services.AddScoped<IRequestHandler<CreateDiscount.Request, Result<DiscountResponse>>, CreateDiscount.Handler>();
        services.AddScoped<IRequestHandler<GetDiscountById.Query, Result<DiscountResponse>>, GetDiscountById.Handler>();
        services.AddScoped<IRequestHandler<GetDiscountPage.Query, Result<PagedResult<DiscountResponse>>>, GetDiscountPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateDiscount.Request, Result<DiscountResponse>>, UpdateDiscount.Handler>();
        services.AddScoped<IRequestHandler<DeleteDiscount.Command, Result<DeleteDiscount.Response>>, DeleteDiscount.Handler>();
        services.AddScoped<IRequestHandler<CreateFeeStructure.Request, Result<FeeStructureResponse>>, CreateFeeStructure.Handler>();
        services.AddScoped<IRequestHandler<GetFeeStructureById.Query, Result<FeeStructureResponse>>, GetFeeStructureById.Handler>();
        services.AddScoped<IRequestHandler<GetFeeStructurePage.Query, Result<PagedResult<FeeStructureResponse>>>, GetFeeStructurePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateFeeStructure.Request, Result<FeeStructureResponse>>, UpdateFeeStructure.Handler>();
        services.AddScoped<IRequestHandler<DeleteFeeStructure.Command, Result<DeleteFeeStructure.Response>>, DeleteFeeStructure.Handler>();
        services.AddScoped<IRequestHandler<CreateFeeType.Request, Result<FeeTypeResponse>>, CreateFeeType.Handler>();
        services.AddScoped<IRequestHandler<GetFeeTypeById.Query, Result<FeeTypeResponse>>, GetFeeTypeById.Handler>();
        services.AddScoped<IRequestHandler<GetFeeTypePage.Query, Result<PagedResult<FeeTypeResponse>>>, GetFeeTypePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateFeeType.Request, Result<FeeTypeResponse>>, UpdateFeeType.Handler>();
        services.AddScoped<IRequestHandler<DeleteFeeType.Command, Result<DeleteFeeType.Response>>, DeleteFeeType.Handler>();
        services.AddScoped<IRequestHandler<CreateInvoice.Request, Result<InvoiceResponse>>, CreateInvoice.Handler>();
        services.AddScoped<IRequestHandler<GetInvoiceById.Query, Result<InvoiceResponse>>, GetInvoiceById.Handler>();
        services.AddScoped<IRequestHandler<GetInvoicePage.Query, Result<PagedResult<InvoiceResponse>>>, GetInvoicePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateInvoice.Request, Result<InvoiceResponse>>, UpdateInvoice.Handler>();
        services.AddScoped<IRequestHandler<DeleteInvoice.Command, Result<DeleteInvoice.Response>>, DeleteInvoice.Handler>();
        services.AddScoped<IRequestHandler<CreatePayment.Request, Result<PaymentResponse>>, CreatePayment.Handler>();
        services.AddScoped<IRequestHandler<GetPaymentById.Query, Result<PaymentResponse>>, GetPaymentById.Handler>();
        services.AddScoped<IRequestHandler<GetPaymentPage.Query, Result<PagedResult<PaymentResponse>>>, GetPaymentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdatePayment.Request, Result<PaymentResponse>>, UpdatePayment.Handler>();
        services.AddScoped<IRequestHandler<DeletePayment.Command, Result<DeletePayment.Response>>, DeletePayment.Handler>();
        services.AddScoped<IRequestHandler<CreateScholarship.Request, Result<ScholarshipResponse>>, CreateScholarship.Handler>();
        services.AddScoped<IRequestHandler<GetScholarshipById.Query, Result<ScholarshipResponse>>, GetScholarshipById.Handler>();
        services.AddScoped<IRequestHandler<GetScholarshipPage.Query, Result<PagedResult<ScholarshipResponse>>>, GetScholarshipPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateScholarship.Request, Result<ScholarshipResponse>>, UpdateScholarship.Handler>();
        services.AddScoped<IRequestHandler<DeleteScholarship.Command, Result<DeleteScholarship.Response>>, DeleteScholarship.Handler>();
        services.AddScoped<IRequestHandler<CreateStudentFee.Request, Result<StudentFeeResponse>>, CreateStudentFee.Handler>();
        services.AddScoped<IRequestHandler<GetStudentFeeById.Query, Result<StudentFeeResponse>>, GetStudentFeeById.Handler>();
        services.AddScoped<IRequestHandler<GetStudentFeePage.Query, Result<PagedResult<StudentFeeResponse>>>, GetStudentFeePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateStudentFee.Request, Result<StudentFeeResponse>>, UpdateStudentFee.Handler>();
        services.AddScoped<IRequestHandler<DeleteStudentFee.Command, Result<DeleteStudentFee.Response>>, DeleteStudentFee.Handler>();

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
