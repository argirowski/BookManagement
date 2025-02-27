using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BookDTO>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AddBookCommandHandler(IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var bookCategories = categories
                .Where(c => request.CategoryNames.Contains(c.Name))
                .Select(c => new BookCategory { Category = c })
                .ToList();

            var book = new Book
            {
                BookId = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                PublishedDate = request.PublishedDate,
                BookCategories = bookCategories
            };

            await _bookRepository.AddBookAsync(book);

            return _mapper.Map<BookDTO>(book);
        }
    }
}
