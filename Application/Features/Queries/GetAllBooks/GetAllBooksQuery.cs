using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<IEnumerable<BookDTO>>
    {
    }
}
