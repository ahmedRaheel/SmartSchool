using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Features.Document;
using SmartSchool.Modules.Documents.Features.DocumentSetup;
using SmartSchool.Modules.Documents.Features.DownloadDocument;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents;

public static class Module
{
    public static IServiceCollection AddDocumentsModule(
        this IServiceCollection services)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddScoped<IDocumentsDbContext>(serviceProvider =>
            serviceProvider.GetRequiredService<DocumentsDbContext>());
        services.AddFeaturePersistence(typeof(Module).Assembly);

        return services;
    }

    public static IEndpointRouteBuilder MapDocumentsEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateDocument.MapEndpoint(endpoints);
        GetDocumentById.MapEndpoint(endpoints);
        GetDocumentPage.MapEndpoint(endpoints);
        UpdateDocument.MapEndpoint(endpoints);
        DeleteDocument.MapEndpoint(endpoints);
        DownloadDocument.MapEndpoint(endpoints);

        GetDocumentSetup.MapEndpoint(endpoints);
        CreateDocumentType.MapEndpoint(endpoints);
        CreateRequiredDocumentType.MapEndpoint(endpoints);
        CreateRequiredDocument.MapEndpoint(endpoints);

        return endpoints;
    }
}
