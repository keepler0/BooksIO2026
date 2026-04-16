using BooksIO2026.Entities;
using FluentValidation;

namespace BooksIO2026.Service.Validators
{
    public class PublisherValidator : AbstractValidator<Publisher>
    {
        public PublisherValidator()
        {
            RuleFor(p => p.Name).NotEmpty()
                                .WithMessage("Name is required")
                                .MaximumLength(100)
                                .MinimumLength(3)
                                .WithMessage("The name must be between 3 and 100 characters");

            RuleFor(p => p.Country).NotEmpty()
                                   .WithMessage("Country is required")
                                   .MaximumLength(60)
                                   .MinimumLength(3)
                                   .WithMessage("The country must be between 3 and 60 characters");

            RuleFor(p => p.FoundedDate).NotEmpty()
                                       .LessThanOrEqualTo(DateTime.Now);
        }
    }
}
