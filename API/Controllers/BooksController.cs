using Application.DTOs;
using Application.Features.Commands.AddBook;
using Application.Features.Commands.DeleteBook;
using Application.Features.Commands.UpdateBook;
using Application.Features.Queries.GetAllBooks;
using Application.Features.Queries.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper? _mapper;

        public BooksController(IMediator mediator, IMapper? mapper = null)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(Guid id)
        {
            var query = new GetBookByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> AddBook(AddBookDTO dto)
        {
            var command = _mapper.Map<AddBookCommand>(dto);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBookById), new { id = result.BookId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(Guid id, UpdateBookDTO dto)
        {
            var command = _mapper.Map<UpdateBookCommand>(dto);
            command.BookId = id; // Ensure the command has the correct ID
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var command = new DeleteBookCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
