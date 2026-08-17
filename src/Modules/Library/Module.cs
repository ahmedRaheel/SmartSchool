
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Persistence;

using SmartSchool.Modules.Library.Features.Book;
using SmartSchool.Modules.Library.Features.BookCopy;
using SmartSchool.Modules.Library.Features.Loan;
using SmartSchool.Modules.Library.Features.Reservation;

namespace SmartSchool.Modules.Library;

public static class Module
{
    public static IServiceCollection AddLibraryModule(
        this IServiceCollection services)
    {
        services.AddScoped<IBookQuery, BookQuery>();
        services.AddScoped<IBookCommand, BookCommand>();
        services.AddScoped<IBookCopyQuery, BookCopyQuery>();
        services.AddScoped<IBookCopyCommand, BookCopyCommand>();
        services.AddScoped<ILoanQuery, LoanQuery>();
        services.AddScoped<ILoanCommand, LoanCommand>();
        services.AddScoped<IReservationQuery, ReservationQuery>();
        services.AddScoped<IReservationCommand, ReservationCommand>();

        return services;
    }

    public static IEndpointRouteBuilder MapLibraryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        CreateBook.MapEndpoint(endpoints);
        GetBookById.MapEndpoint(endpoints);
        GetBookPage.MapEndpoint(endpoints);
        UpdateBook.MapEndpoint(endpoints);
        DeleteBook.MapEndpoint(endpoints);
        CreateBookCopy.MapEndpoint(endpoints);
        GetBookCopyById.MapEndpoint(endpoints);
        GetBookCopyPage.MapEndpoint(endpoints);
        UpdateBookCopy.MapEndpoint(endpoints);
        DeleteBookCopy.MapEndpoint(endpoints);
        CreateLoan.MapEndpoint(endpoints);
        GetLoanById.MapEndpoint(endpoints);
        GetLoanPage.MapEndpoint(endpoints);
        UpdateLoan.MapEndpoint(endpoints);
        DeleteLoan.MapEndpoint(endpoints);
        CreateReservation.MapEndpoint(endpoints);
        GetReservationById.MapEndpoint(endpoints);
        GetReservationPage.MapEndpoint(endpoints);
        UpdateReservation.MapEndpoint(endpoints);
        DeleteReservation.MapEndpoint(endpoints);

        return endpoints;
    }
}
