using FluentValidation;
using SmartSchool.Modules.Documents.Features.Certificate;
using SmartSchool.Modules.Documents.Features.DocumentTemplate;
using SmartSchool.Modules.Documents.Features.GeneratedDocument;
using SmartSchool.Modules.Documents.Features.SchoolLogo;

namespace SmartSchool.Modules.Documents;

public static class Module
{
    public static IServiceCollection AddDocumentsModule(
        this IServiceCollection services)
    {
        services.AddScoped<CreateCertificate.Handler>();
        services.AddScoped<GetCertificateById.Handler>();
        services.AddScoped<GetCertificatePage.Handler>();
        services.AddScoped<UpdateCertificate.Handler>();
        services.AddScoped<DeleteCertificate.Handler>();
        services.AddScoped<IValidator<CreateCertificate.Request>, CreateCertificate.Validator>();
        services.AddScoped<IValidator<UpdateCertificate.Request>, UpdateCertificate.Validator>();
        services.AddScoped<CreateDocumentTemplate.Handler>();
        services.AddScoped<GetDocumentTemplateById.Handler>();
        services.AddScoped<GetDocumentTemplatePage.Handler>();
        services.AddScoped<UpdateDocumentTemplate.Handler>();
        services.AddScoped<DeleteDocumentTemplate.Handler>();
        services.AddScoped<IValidator<CreateDocumentTemplate.Request>, CreateDocumentTemplate.Validator>();
        services.AddScoped<IValidator<UpdateDocumentTemplate.Request>, UpdateDocumentTemplate.Validator>();
        services.AddScoped<CreateGeneratedDocument.Handler>();
        services.AddScoped<GetGeneratedDocumentById.Handler>();
        services.AddScoped<GetGeneratedDocumentPage.Handler>();
        services.AddScoped<UpdateGeneratedDocument.Handler>();
        services.AddScoped<DeleteGeneratedDocument.Handler>();
        services.AddScoped<IValidator<CreateGeneratedDocument.Request>, CreateGeneratedDocument.Validator>();
        services.AddScoped<IValidator<UpdateGeneratedDocument.Request>, UpdateGeneratedDocument.Validator>();
        services.AddScoped<CreateSchoolLogo.Handler>();
        services.AddScoped<GetSchoolLogoById.Handler>();
        services.AddScoped<GetSchoolLogoPage.Handler>();
        services.AddScoped<UpdateSchoolLogo.Handler>();
        services.AddScoped<DeleteSchoolLogo.Handler>();
        services.AddScoped<IValidator<CreateSchoolLogo.Request>, CreateSchoolLogo.Validator>();
        services.AddScoped<IValidator<UpdateSchoolLogo.Request>, UpdateSchoolLogo.Validator>();

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
