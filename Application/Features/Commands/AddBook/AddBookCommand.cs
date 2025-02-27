using Application.DTOs;
using MediatR;

namespace Application.Features.Commands.AddBook
{
    public class AddBookCommand : IRequest<BookDTO>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<string> CategoryNames { get; set; }
    }
}
