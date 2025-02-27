using Application.Features.Commands.UpdateBook;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateBookCommandValidator : BaseValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator(IBookRepository bookRepository, ICategoryRepository categoryRepository)
            : base(bookRepository, categoryRepository)
        {
            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("Book ID is required.")
                .Must(BookExists).WithMessage("The specified book does not exist.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must((command, title) => !UpdatedBookTitleExists(command.BookId, title)).WithMessage("A book with the same title already exists.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Published Date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Published Date cannot be in the future.");


            RuleFor(x => x.CategoryNames)
                .NotEmpty().WithMessage("At least one category is required.")
                .Must(CategoriesExist).WithMessage("One or more categories do not exist.");
        }
    }
}