using BooksIO2026.Entities;
using FluentValidation;

namespace BooksIO2026.Service.Validators
{
    public class AuthorValidator : AbstractValidator<Author>//clase heredada por FluentValidation que se encarga de validar la clase Author
    {
        public AuthorValidator()
        {
            //validaciones para el campo FirstName y LastName,
            //se pueden agregar mas validaciones como por ejemplo que no se repitan los nombres de los autores, etc.
            RuleFor(a => a.FirstName).NotEmpty()
                                     .WithMessage($"First name is required")
                                     .MaximumLength(50)
                                     .MinimumLength(3)
                                     .WithMessage("First name must be between 3 and 50 characters");

            RuleFor(a => a.LastName).NotEmpty()
                                     .WithMessage($"Last name is required")
                                     .MaximumLength(50)
                                     .MinimumLength(3)
                                     .WithMessage("Last name must be between 3 and 50 characters");
        }
    }
}
