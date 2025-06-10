using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResultDTO<BookDTO>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<PagedResultDTO<BookDTO>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllBooksAsync();
            var totalCount = books.Count();
            var pagedBooks = books
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            var result = new PagedResultDTO<BookDTO>
            {
                Items = _mapper.Map<List<BookDTO>>(pagedBooks),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
            return result;
        }
    }
}
