using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Finance.Features.FeeType;
using SmartSchool.Modules.Finance.Features.Invoice;
using SmartSchool.Modules.Finance.Features.Payment;
using SmartSchool.Modules.Finance.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance;

public static class Module
{
	public static IServiceCollection AddFinanceModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IFeeTypeQuery, FeeTypeQuery>();
		services.AddScoped<IFeeTypeCommand, FeeTypeCommand>();
		services.AddScoped<IInvoiceQuery, InvoiceQuery>();
		services.AddScoped<IInvoiceCommand, InvoiceCommand>();
		services.AddScoped<IPaymentQuery, PaymentQuery>();
		services.AddScoped<IPaymentCommand, PaymentCommand>();
		services.AddScoped<IDiscountCommand, DiscountCommand>();
		services.AddScoped<IDiscountQuery, DiscountQuery>();
		services.AddScoped<IFeeStructureCommand, FeeStructureCommand>();
		services.AddScoped<IFeeStructureQuery, FeeStructureQuery>();
		services.AddScoped<IScholarshipCommand, ScholarshipCommand>();
		services.AddScoped<IScholarshipQuery, ScholarshipQuery>();
		services.AddScoped<IStudentFeeCommand, StudentFeeCommand>();
		services.AddScoped<IStudentFeeQuery, StudentFeeQuery>();
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

		return endpoints;
	}
}
