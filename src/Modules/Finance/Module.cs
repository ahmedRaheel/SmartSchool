using SmartSchool.Modules.Finance.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Features.FeeType;
using SmartSchool.Modules.Finance.Features.Invoice;
using SmartSchool.Modules.Finance.Features.Payment;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Finance.Features.Discount;
using SmartSchool.Modules.Finance.Features.FeeStructure;
using SmartSchool.Modules.Finance.Features.Scholarship;
using SmartSchool.Modules.Finance.Features.StudentFee;

namespace SmartSchool.Modules.Finance;

public static class Module
{
    public static IServiceCollection AddFinanceModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IFinanceDbContext, FinanceDbContext>();

        services.AddFeaturePersistence(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapFinanceEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
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

        CreateDiscount.MapEndpoint(endpoints);
        CreateFeeStructure.MapEndpoint(endpoints);
        CreateScholarship.MapEndpoint(endpoints);
        CreateStudentFee.MapEndpoint(endpoints);
        DeleteDiscount.MapEndpoint(endpoints);
        DeleteFeeStructure.MapEndpoint(endpoints);
        DeleteScholarship.MapEndpoint(endpoints);
        DeleteStudentFee.MapEndpoint(endpoints);
        GetDiscountById.MapEndpoint(endpoints);
        GetDiscountPage.MapEndpoint(endpoints);
        GetFeeStructureById.MapEndpoint(endpoints);
        GetFeeStructurePage.MapEndpoint(endpoints);
        GetScholarshipById.MapEndpoint(endpoints);
        GetScholarshipPage.MapEndpoint(endpoints);
        GetStudentFeeById.MapEndpoint(endpoints);
        GetStudentFeePage.MapEndpoint(endpoints);
        UpdateDiscount.MapEndpoint(endpoints);
        UpdateFeeStructure.MapEndpoint(endpoints);
        UpdateScholarship.MapEndpoint(endpoints);
        UpdateStudentFee.MapEndpoint(endpoints);

        return endpoints;
    }
}
