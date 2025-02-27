using Application.DTOs;
using MediatR;

namespace Application.Features.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDTO>
    {
        public Guid BookId { get; set; }

        public GetBookByIdQuery(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
