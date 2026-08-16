using FluentValidation;
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
        services.AddScoped<CreateBook.Handler>();
        services.AddScoped<GetBookById.Handler>();
        services.AddScoped<GetBookPage.Handler>();
        services.AddScoped<UpdateBook.Handler>();
        services.AddScoped<DeleteBook.Handler>();
        services.AddScoped<IValidator<CreateBook.Request>, CreateBook.Validator>();
        services.AddScoped<IValidator<UpdateBook.Request>, UpdateBook.Validator>();
        services.AddScoped<CreateBookCopy.Handler>();
        services.AddScoped<GetBookCopyById.Handler>();
        services.AddScoped<GetBookCopyPage.Handler>();
        services.AddScoped<UpdateBookCopy.Handler>();
        services.AddScoped<DeleteBookCopy.Handler>();
        services.AddScoped<IValidator<CreateBookCopy.Request>, CreateBookCopy.Validator>();
        services.AddScoped<IValidator<UpdateBookCopy.Request>, UpdateBookCopy.Validator>();
        services.AddScoped<CreateLoan.Handler>();
        services.AddScoped<GetLoanById.Handler>();
        services.AddScoped<GetLoanPage.Handler>();
        services.AddScoped<UpdateLoan.Handler>();
        services.AddScoped<DeleteLoan.Handler>();
        services.AddScoped<IValidator<CreateLoan.Request>, CreateLoan.Validator>();
        services.AddScoped<IValidator<UpdateLoan.Request>, UpdateLoan.Validator>();
        services.AddScoped<CreateReservation.Handler>();
        services.AddScoped<GetReservationById.Handler>();
        services.AddScoped<GetReservationPage.Handler>();
        services.AddScoped<UpdateReservation.Handler>();
        services.AddScoped<DeleteReservation.Handler>();
        services.AddScoped<IValidator<CreateReservation.Request>, CreateReservation.Validator>();
        services.AddScoped<IValidator<UpdateReservation.Request>, UpdateReservation.Validator>();

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
