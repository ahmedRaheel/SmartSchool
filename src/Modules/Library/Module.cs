using SmartSchool.Modules.Library.Contracts;
using SmartSchool.SharedKernel;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Library.Persistence;
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
        services.AddScoped<IBookQuery, BookQuery>();
        services.AddScoped<IBookCommand, BookCommand>();
        services.AddScoped<IBookCopyQuery, BookCopyQuery>();
        services.AddScoped<IBookCopyCommand, BookCopyCommand>();
        services.AddScoped<ILoanQuery, LoanQuery>();
        services.AddScoped<ILoanCommand, LoanCommand>();
        services.AddScoped<IReservationQuery, ReservationQuery>();
        services.AddScoped<IReservationCommand, ReservationCommand>();
        services.AddScoped<IValidator<CreateBook.Request>, CreateBook.Validator>();
        services.AddScoped<IValidator<UpdateBook.Request>, UpdateBook.Validator>();
        services.AddScoped<IValidator<CreateBookCopy.Request>, CreateBookCopy.Validator>();
        services.AddScoped<IValidator<UpdateBookCopy.Request>, UpdateBookCopy.Validator>();
        services.AddScoped<IValidator<CreateLoan.Request>, CreateLoan.Validator>();
        services.AddScoped<IValidator<UpdateLoan.Request>, UpdateLoan.Validator>();
        services.AddScoped<IValidator<CreateReservation.Request>, CreateReservation.Validator>();
        services.AddScoped<IValidator<UpdateReservation.Request>, UpdateReservation.Validator>();


        services.AddScoped<IRequestHandler<CreateBook.Request, Result<BookResponse>>, CreateBook.Handler>();
        services.AddScoped<IRequestHandler<GetBookById.Query, Result<BookResponse>>, GetBookById.Handler>();
        services.AddScoped<IRequestHandler<GetBookPage.Query, Result<PagedResult<BookResponse>>>, GetBookPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateBook.Request, Result<BookResponse>>, UpdateBook.Handler>();
        services.AddScoped<IRequestHandler<DeleteBook.Command, Result<DeleteBook.Response>>, DeleteBook.Handler>();
        services.AddScoped<IRequestHandler<CreateBookCopy.Request, Result<BookCopyResponse>>, CreateBookCopy.Handler>();
        services.AddScoped<IRequestHandler<GetBookCopyById.Query, Result<BookCopyResponse>>, GetBookCopyById.Handler>();
        services.AddScoped<IRequestHandler<GetBookCopyPage.Query, Result<PagedResult<BookCopyResponse>>>, GetBookCopyPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateBookCopy.Request, Result<BookCopyResponse>>, UpdateBookCopy.Handler>();
        services.AddScoped<IRequestHandler<DeleteBookCopy.Command, Result<DeleteBookCopy.Response>>, DeleteBookCopy.Handler>();
        services.AddScoped<IRequestHandler<CreateLoan.Request, Result<LoanResponse>>, CreateLoan.Handler>();
        services.AddScoped<IRequestHandler<GetLoanById.Query, Result<LoanResponse>>, GetLoanById.Handler>();
        services.AddScoped<IRequestHandler<GetLoanPage.Query, Result<PagedResult<LoanResponse>>>, GetLoanPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateLoan.Request, Result<LoanResponse>>, UpdateLoan.Handler>();
        services.AddScoped<IRequestHandler<DeleteLoan.Command, Result<DeleteLoan.Response>>, DeleteLoan.Handler>();
        services.AddScoped<IRequestHandler<CreateReservation.Request, Result<ReservationResponse>>, CreateReservation.Handler>();
        services.AddScoped<IRequestHandler<GetReservationById.Query, Result<ReservationResponse>>, GetReservationById.Handler>();
        services.AddScoped<IRequestHandler<GetReservationPage.Query, Result<PagedResult<ReservationResponse>>>, GetReservationPage.Handler>();
        services.AddScoped<IRequestHandler<UpdateReservation.Request, Result<ReservationResponse>>, UpdateReservation.Handler>();
        services.AddScoped<IRequestHandler<DeleteReservation.Command, Result<DeleteReservation.Response>>, DeleteReservation.Handler>();

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
