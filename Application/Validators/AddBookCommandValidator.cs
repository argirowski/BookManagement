using Application.Features.Commands.AddBook;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class AddBookCommandValidator : BaseValidator<AddBookCommand>
    {
        public AddBookCommandValidator(IBookRepository bookRepository, ICategoryRepository categoryRepository)
            : base(bookRepository, categoryRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Must(title => !AddedBookTitleExists(title)).WithMessage("A book with the same title already exists.");

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