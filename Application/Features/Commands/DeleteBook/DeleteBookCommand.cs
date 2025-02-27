using MediatR;

namespace Application.Features.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public Guid BookId { get; set; }

        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }
    }
}
