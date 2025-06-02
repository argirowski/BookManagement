using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.AddBook
{
    public class AddBookCommand : IRequest<BookDTO>
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required DateTime PublishedDate { get; set; }
        public required List<string> CategoryNames { get; set; }
    }
}
