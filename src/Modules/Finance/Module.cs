using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Features.Discount;
using SmartSchool.Modules.Finance.Features.FeeStructure;
using SmartSchool.Modules.Finance.Features.FeeType;
using SmartSchool.Modules.Finance.Features.Invoice;
using SmartSchool.Modules.Finance.Features.Payment;
using SmartSchool.Modules.Finance.Features.Scholarship;
using SmartSchool.Modules.Finance.Features.StudentFee;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance;

public static class Module
{
	public static IServiceCollection AddFinanceModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
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
