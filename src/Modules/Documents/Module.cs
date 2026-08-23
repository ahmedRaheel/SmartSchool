using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Features.DocumentTemplate;
using SmartSchool.Modules.Documents.Features.GeneratedDocument;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents;

public static class Module
{
	public static IServiceCollection AddDocumentsModule(
		this IServiceCollection services)
	{
		services.AddSmartSchoolMediator(typeof(Module).Assembly);
		services.AddScoped<IDocumentTemplateQuery, DocumentTemplateQuery>();
		services.AddScoped<IDocumentTemplateCommand, DocumentTemplateCommand>();
		services.AddScoped<IGeneratedDocumentQuery, GeneratedDocumentQuery>();
		services.AddScoped<IGeneratedDocumentCommand, GeneratedDocumentCommand>();

		return services;
	}

	public static IEndpointRouteBuilder MapDocumentsEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		CreateDocumentTemplate.MapEndpoint(endpoints);
		GetDocumentTemplateById.MapEndpoint(endpoints);
		GetDocumentTemplatePage.MapEndpoint(endpoints);
		UpdateDocumentTemplate.MapEndpoint(endpoints);
		DeleteDocumentTemplate.MapEndpoint(endpoints);
		CreateGeneratedDocument.MapEndpoint(endpoints);
		GetGeneratedDocumentById.MapEndpoint(endpoints);
		GetGeneratedDocumentPage.MapEndpoint(endpoints);
		UpdateGeneratedDocument.MapEndpoint(endpoints);
		DeleteGeneratedDocument.MapEndpoint(endpoints);

		return endpoints;
	}
}
