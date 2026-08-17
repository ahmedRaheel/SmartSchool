using SmartSchool.Modules.Documents.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Documents.Persistence;
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
        services.AddScoped<ICertificateQuery, CertificateQuery>();
        services.AddScoped<ICertificateCommand, CertificateCommand>();
        services.AddScoped<IDocumentTemplateQuery, DocumentTemplateQuery>();
        services.AddScoped<IDocumentTemplateCommand, DocumentTemplateCommand>();
        services.AddScoped<IGeneratedDocumentQuery, GeneratedDocumentQuery>();
        services.AddScoped<IGeneratedDocumentCommand, GeneratedDocumentCommand>();
        services.AddScoped<ISchoolLogoQuery, SchoolLogoQuery>();
        services.AddScoped<ISchoolLogoCommand, SchoolLogoCommand>();
        services.AddScoped<IValidator<CreateCertificate.Request>, CreateCertificate.Validator>();
        services.AddScoped<IValidator<UpdateCertificate.Request>, UpdateCertificate.Validator>();
        services.AddScoped<IValidator<CreateDocumentTemplate.Request>, CreateDocumentTemplate.Validator>();
        services.AddScoped<IValidator<UpdateDocumentTemplate.Request>, UpdateDocumentTemplate.Validator>();
        services.AddScoped<IValidator<CreateGeneratedDocument.Request>, CreateGeneratedDocument.Validator>();
        services.AddScoped<IValidator<UpdateGeneratedDocument.Request>, UpdateGeneratedDocument.Validator>();
        services.AddScoped<IValidator<CreateSchoolLogo.Request>, CreateSchoolLogo.Validator>();
        services.AddScoped<IValidator<UpdateSchoolLogo.Request>, UpdateSchoolLogo.Validator>();


        services.AddScoped<IRequestHandler<CreateCertificate.Request, Result<CertificateResponse>>, CreateCertificate.Handler>();
        services.AddScoped<IRequestHandler<GetCertificateById.Query, Result<CertificateResponse>>, GetCertificateById.Handler>();
        services.AddScoped<IRequestHandler<GetCertificatePage.Query, Result<PagedResult<CertificateResponse>>>, GetCertificatePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateCertificate.Request, Result<CertificateResponse>>, UpdateCertificate.Handler>();
        services.AddScoped<IRequestHandler<DeleteCertificate.Command, Result<DeleteCertificate.Response>>, DeleteCertificate.Handler>();
        services.AddScoped<IRequestHandler<CreateDocumentTemplate.Request, Result<DocumentTemplateResponse>>, CreateDocumentTemplate.Handler>();
        services.AddScoped<IRequestHandler<GetDocumentTemplateById.Query, Result<DocumentTemplateResponse>>, GetDocumentTemplateById.Handler>();
        services.AddScoped<IRequestHandler<GetDocumentTemplatePage.Query, Result<PagedResult<DocumentTemplateResponse>>>, GetDocumentTemplatePage.Handler>();
        services.AddScoped<IRequestHandler<UpdateDocumentTemplate.Request, Result<DocumentTemplateResponse>>, UpdateDocumentTemplate.Handler>();
        services.AddScoped<IRequestHandler<DeleteDocumentTemplate.Command, Result<DeleteDocumentTemplate.Response>>, DeleteDocumentTemplate.Handler>();
        services.AddScoped<IRequestHandler<CreateGeneratedDocument.Request, Result<GeneratedDocumentResponse>>, CreateGeneratedDocument.Handler>();
        services.AddScoped<IRequestHandler<GetGeneratedDocumentById.Query, Result<GeneratedDocumentResponse>>, GetGeneratedDocumentById.Handler>();
        services.AddScoped<IRequestHandler<GetGeneratedDocumentPage.Query, Result<PagedResult<GeneratedDocumentResponse>>>, GetGeneratedDocumentPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateGeneratedDocument.Request, Result<GeneratedDocumentResponse>>, UpdateGeneratedDocument.Handler>();
        services.AddScoped<IRequestHandler<DeleteGeneratedDocument.Command, Result<DeleteGeneratedDocument.Response>>, DeleteGeneratedDocument.Handler>();
        services.AddScoped<IRequestHandler<CreateSchoolLogo.Request, Result<SchoolLogoResponse>>, CreateSchoolLogo.Handler>();
        services.AddScoped<IRequestHandler<GetSchoolLogoById.Query, Result<SchoolLogoResponse>>, GetSchoolLogoById.Handler>();
        services.AddScoped<IRequestHandler<GetSchoolLogoPage.Query, Result<PagedResult<SchoolLogoResponse>>>, GetSchoolLogoPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateSchoolLogo.Request, Result<SchoolLogoResponse>>, UpdateSchoolLogo.Handler>();
        services.AddScoped<IRequestHandler<DeleteSchoolLogo.Command, Result<DeleteSchoolLogo.Response>>, DeleteSchoolLogo.Handler>();

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
