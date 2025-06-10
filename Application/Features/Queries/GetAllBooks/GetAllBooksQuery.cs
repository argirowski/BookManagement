using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<PagedResultDTO<BookDTO>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
