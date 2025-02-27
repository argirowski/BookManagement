using Application.Features.Commands.DeleteBook;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class DeleteBookCommandValidator : BaseValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator(IBookRepository bookRepository)
            : base(bookRepository, null)
        {
            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("Book ID is required.")
                .Must(BookExists).WithMessage("The specified book does not exist.");
        }
    }
}