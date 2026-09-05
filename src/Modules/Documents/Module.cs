using SmartSchool.Modules.Documents.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Features.DocumentTemplate;
using SmartSchool.Modules.Documents.Features.GeneratedDocument;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Documents.Features;
using SmartSchool.Modules.Documents.Features.Certificate;
using SmartSchool.Modules.Documents.Features.SchoolLogo;
namespace SmartSchool.Modules.Documents;

public static class Module
{
    public static IServiceCollection AddDocumentsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IDocumentsDbContext, DocumentsDbContext>();

        services.AddFeaturePersistence(typeof(Module).Assembly);
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

        CreateCertificate.MapEndpoint(endpoints);
        CreateSchoolLogo.MapEndpoint(endpoints);
        DeleteCertificate.MapEndpoint(endpoints);
        DeleteSchoolLogo.MapEndpoint(endpoints);
        GetCertificateById.MapEndpoint(endpoints);
        GetCertificatePage.MapEndpoint(endpoints);
        GetSchoolLogoById.MapEndpoint(endpoints);
        GetSchoolLogoPage.MapEndpoint(endpoints);
        UpdateCertificate.MapEndpoint(endpoints);
        UpdateSchoolLogo.MapEndpoint(endpoints);
        DocumentManagementEndpoints.MapDocumentManagement(endpoints);

        return endpoints;
    }
}
