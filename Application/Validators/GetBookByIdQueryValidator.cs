﻿using Application.Features.Queries.GetBookById;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Validators
{
    public class GetBookByIdQueryValidator : BaseValidator<GetBookByIdQuery>
    {
        public GetBookByIdQueryValidator(IBookRepository bookRepository)
            : base(bookRepository, null)
        {
            RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("Book ID is required.")
                .Must(BookExists).WithMessage("The specified book does not exist.");
        }
    }
}