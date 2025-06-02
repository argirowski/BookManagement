using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDTO>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<BookDTO> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);

            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var bookCategories = categories
                .Where(c => request.CategoryNames.Contains(c.Name))
                .Select(c => new BookCategory { Category = c })
                .ToList();

            book.Title = request.Title;
            book.Author = request.Author;
            book.PublishedDate = request.PublishedDate;
            book.BookCategories = bookCategories;

            await _bookRepository.UpdateBookAsync(book);

            return _mapper.Map<BookDTO>(book);
        }
    }
}
