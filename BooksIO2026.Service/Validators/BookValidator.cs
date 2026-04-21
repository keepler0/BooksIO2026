using BooksIO2026.Entities;
using FluentValidation;

namespace BooksIO2026.Service.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title).NotEmpty()
                                 .WithMessage("The title is required")
                                 .MaximumLength(100)
                                 .MinimumLength(3)
                                 .WithMessage("The title must be between 3 and 100 characters");
            RuleFor(b => b.Price).NotEmpty()
                                 .WithMessage("The price is required");
            RuleFor(b => b.PublishedDate).NotEmpty()
                                         .WithMessage("The published date is required");
        }
    }
}
