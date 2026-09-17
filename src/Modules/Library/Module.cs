using SmartSchool.Modules.Library.Persistence;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Features.Book;
using SmartSchool.Modules.Library.Features.BookCopy;
using SmartSchool.Modules.Library.Features.Loan;
using SmartSchool.Modules.Library.Features.Operations;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Library.Features.Reservation;
namespace SmartSchool.Modules.Library;

public static class Module
{
    public static IServiceCollection AddLibraryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSmartSchoolMediator(typeof(Module).Assembly);
        services.AddModuleDbContext<LibraryDbContext, ILibraryDbContext>(
            configuration,
            ModuleConstants.Schema);

        services.AddFeaturePersistence(typeof(Module).Assembly);
        return services;
    }

    public static IEndpointRouteBuilder MapLibraryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        GetBookById.MapEndpoint(endpoints);
        GetBookPage.MapEndpoint(endpoints);
        UpdateBook.MapEndpoint(endpoints);
        DeleteBook.MapEndpoint(endpoints);
        GetBookCopyById.MapEndpoint(endpoints);
        GetBookCopyByCampusId.MapEndpoint(endpoints);
        GetBookCopyByBookId.MapEndpoint(endpoints);
        GetBookCopyPage.MapEndpoint(endpoints);
        GetLoanById.MapEndpoint(endpoints);
        GetLoanByStudentId.MapEndpoint(endpoints);
        GetLoanByEmployeeId.MapEndpoint(endpoints);
        GetLoanByBookCopyId.MapEndpoint(endpoints);
        GetLoanPage.MapEndpoint(endpoints);
        LibraryOperations.MapEndpoints(endpoints);

        CreateReservation.MapEndpoint(endpoints);
        DeleteReservation.MapEndpoint(endpoints);
        GetReservationById.MapEndpoint(endpoints);
        GetReservationPage.MapEndpoint(endpoints);
        UpdateReservation.MapEndpoint(endpoints);

        return endpoints;
    }
}
