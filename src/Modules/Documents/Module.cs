
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Features.Certificate;
using SmartSchool.Modules.Documents.Features.DocumentTemplate;
using SmartSchool.Modules.Documents.Features.GeneratedDocument;
using SmartSchool.Modules.Documents.Features.SchoolLogo;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents;

public static class Module
{
    public static IServiceCollection AddDocumentsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<ICertificateQuery, CertificateQuery>();
        services.AddScoped<ICertificateCommand, CertificateCommand>();
        services.AddScoped<IDocumentTemplateQuery, DocumentTemplateQuery>();
        services.AddScoped<IDocumentTemplateCommand, DocumentTemplateCommand>();
        services.AddScoped<IGeneratedDocumentQuery, GeneratedDocumentQuery>();
        services.AddScoped<IGeneratedDocumentCommand, GeneratedDocumentCommand>();
        services.AddScoped<ISchoolLogoQuery, SchoolLogoQuery>();
        services.AddScoped<ISchoolLogoCommand, SchoolLogoCommand>();

        return services;
    }

    public static IEndpointRouteBuilder MapDocumentsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateCertificate.MapEndpoint(endpoints);
        GetCertificateById.MapEndpoint(endpoints);
        GetCertificatePage.MapEndpoint(endpoints);
        UpdateCertificate.MapEndpoint(endpoints);
        DeleteCertificate.MapEndpoint(endpoints);
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
        CreateSchoolLogo.MapEndpoint(endpoints);
        GetSchoolLogoById.MapEndpoint(endpoints);
        GetSchoolLogoPage.MapEndpoint(endpoints);
        UpdateSchoolLogo.MapEndpoint(endpoints);
        DeleteSchoolLogo.MapEndpoint(endpoints);

        return endpoints;
    }
}
